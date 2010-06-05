// 
// ScrotViewer.cs
//  
// Author:
//       Anirudh Sanjeev <anirudh@anirudhsanjeev.org>
// 
// Copyright (c) 2009 Anirudh Sanjeev
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.IO;
using Gtk;
using System.Collections.Generic;
using Gdk;

namespace ScreenLapse
{

	public partial class ScrotViewer : Gtk.Window
	{
		
		public bool IsPlaying{get;set;}
		public System.Timers.Timer playTimer;
		public ScrotViewer () : base(Gtk.WindowType.Toplevel)
		{
			// Initialize members
			
			validDirectories = new List<string> ();
			this.Build ();
			// Build the treeview
			TreeViewColumn dayColumn = new TreeViewColumn ();
			dayColumn.Title = "Day";
			CellRendererText textRenderer = new CellRendererText ();
			dayColumn.PackStart (textRenderer, true);
			dayColumn.AddAttribute (textRenderer, "text", 0);
			
			dayListStore = new ListStore (typeof(string));
			
			dayAvailTreeView.AppendColumn (dayColumn);
			dayAvailTreeView.Model = dayListStore;
			this.ResizeMode = ResizeMode.Immediate;
			ExtractDayPaths ();
			
			// handle tree activate
			dayAvailTreeView.RowActivated += DayAvailRowActivated;
			
			// handle the time slider
			timeSlider.ValueChanged += RedrawImage;
			ImageArmed = false;
			
			playPauseButton.Clicked += StartPlayPause;
			playTimer = new System.Timers.Timer();
			playTimer.Interval = 500;
			
			playTimer.Elapsed += ImageFrameChangeTimerTick;
		}

		void ImageFrameChangeTimerTick (object sender, System.Timers.ElapsedEventArgs e)
		{
			timeSlider.Value += 1;
		}

		void StartPlayPause (object sender, EventArgs e)
		{
			if(playTimer.Enabled)
			{
				playTimer.Stop();
			}
			else {
				playTimer.Start();
			}
		}
		bool ImageArmed;

		/// <summary>
		/// Callback from the time slider widget. Redraws the appropriate image on the screen
		/// 
		/// Also will make playback easier, because I can just increment the value on the
		/// timeslider and the object will change
		/// </summary>
		/// <param name="sender">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <param name="e">
		/// A <see cref="EventArgs"/>
		/// </param>
		void RedrawImage (object sender, EventArgs e)
		{
			if (ImageArmed) {
				// get the index of the slider
				int index = (int)timeSlider.Value;
				
				// check if the currentfilenames has an index which corresponds to this
				if (currentFileNames == null)
					return;
				if (currentFileNames.Count < index)
					return;
				
				try {
					string filename = currentFileNames[index];
					if (File.Exists (filename)) {
						DrawImageInWindow(filename);
					}
				} catch (Exception ex) {
					Console.WriteLine ("Something broke with exception: " + ex.Message);
					return;
				}
			}
		}

		List<string> currentFileNames;
		
		
		/// <summary>
		/// Placeholder method to draw the existing filename inside the window.
		/// 
		/// Currently, we are using a method where we create a new HBox and stuff
		/// the image widget there and remove the old widget, which is technically overkill
		/// but since this behaves somewhat like a double buffer, it should make the overall
		/// experience smoother.
		/// </summary>
		/// <param name="filename">
		/// A <see cref="System.String"/>
		/// </param>
		void DrawImageInWindow(string filename)
		{
			// Create the new image widget and draw it being hidden
			try {
				Console.WriteLine ("Drawing image inside window");
				Gtk.Image drawTarget = new Gtk.Image(filename);
				drawTarget.Hide();
				
				foreach(Widget child in hbox3.Children)
				{
					// remove all existing children
					hbox3.Remove(child);
					child.Dispose();
				}
				drawTarget.Show();
				hbox3.PackStart(drawTarget, true, true, 0);
				
			} catch (Exception ex) {
				Console.WriteLine ("something went wrong. " + ex.Message);
			}	
			
		}

		void DayAvailRowActivated (object o, RowActivatedArgs args)
		{
			// find the current day path			
			if (currentFileNames != null) {
				currentFileNames.Clear ();
			} else {
				currentFileNames = new List<string> ();
			}
			currentDayPath = System.IO.Path.Combine(Preferences.SavePath, validDirectories[args.Path.Indices[0]]);
			
			// Iterate over all the files in currentDayPath
			foreach (string filename in Directory.GetFiles (currentDayPath)) {
				Console.WriteLine (filename);
				if (System.IO.Path.GetExtension (filename) == ".png" && System.IO.Path.GetFileNameWithoutExtension (filename).Length == 6) {
					currentFileNames.Add (System.IO.Path.GetFullPath(filename));
				}
			}
			Console.WriteLine ("The count of currentfilenames is: " + currentFileNames.Count.ToString ());
			// set up the slider
			if (currentFileNames.Count != 0) {
				timeSlider.SetRange (0, currentFileNames.Count);
				timeSlider.Value = 1;
				
				ImageArmed = true;
			} else {
				timeSlider.SetRange (0, 1);
				ImageArmed = false;
				timeSlider.Value = 0;
			}
			
		}

		ListStore dayListStore;

		List<string> validDirectories;
		string currentDayPath;

		/// <summary>
		/// Since each day is stored as a different path, this function extracts
		/// all the existing day paths in the applicatoins running path
		/// </summary>
		public void ExtractDayPaths ()
		{
			foreach (string dir in Directory.GetDirectories (Preferences.SavePath)) {
				int x;
				// TODO: Should we do this by Regexes?
				// our format uses 12 characters
				if (dir.Length == 12) {
					Console.WriteLine ("Directory: " + dir);
					string dirTrimmed = dir.TrimStart ("./".ToCharArray ());
					DateTime dirDate;
					Console.WriteLine ("The final dir is:" + dirTrimmed);
					try {
						dirDate = DateTime.Parse (dirTrimmed);
					} catch {
						Console.WriteLine ("Datetime parsing failed");
						continue;
						// Don't process ahead
					}
					Console.WriteLine ("The date is {0}", dirDate.ToString ());
					dayListStore.AppendValues (dirDate.ToShortDateString ());
					
					validDirectories.Add (dir);
				}
			}
		}
	}
	
}

// 
// ScrotDaemon.cs
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
using System.Drawing.Imaging;
using System.Drawing;
using Gtk;
using System.Diagnostics;

namespace ScreenLapse
{

	public class ScrotDaemon
	{

		static readonly ScrotDaemon instance = new ScrotDaemon ();

		private System.Timers.Timer timer;

		public static ScrotDaemon Instance {
			get { return instance; }
		}


		/// <summary>
		/// Property to check whether screenshots can be taken
		/// 
		/// use Activate() and Deactivate() to modify behaviour
		/// <c>
		/// </c>
		/// </summary>
		public bool IsActive { get; private set; }

		/// <summary>
		/// Activate the screenshot timer
		/// </summary>
		public void Activate ()
		{
			IsActive = true;
			timer.Enabled = true;
			Preferences.Enabled = true;
		}
		
		/// <summary>
		/// Deactivate the screenshot timer
		/// </summary>
		public void Deactivate ()
		{
			IsActive = false;
			timer.Enabled = false;
			Preferences.Enabled = false;
		}

		private ScrotDaemon ()
		{
			timer = new System.Timers.Timer ();
			timer.Elapsed += OnTimerTick;
			
			timer.Interval = Preferences.Interval;
			
			GC.KeepAlive (timer);
			Deactivate();
		}
		

		void OnTimerTick (object sender, System.Timers.ElapsedEventArgs e)
		{
			// Set the directory name and file name as [MMDDYYYY/HHMMSS.png]
			// the format of the integer to be printed out
			
			//string dirName = String.Format("{0}{1}{2}", e.SignalTime.Date.Month.ToString(specifier), e.SignalTime.Date.Day.ToString(specifier), e.SignalTime.Date.Year.ToString(specifier));
			//string fileName = String.Format("{0}{1}{2}.png", e.SignalTime.Hour.ToString(specifier), e.SignalTime.Minute.ToString(specifier), e.SignalTime.Second.ToString(specifier));
			
			// update the timer
			timer.Interval = Preferences.Interval;
			
			// check whether there has been any idleness
			ProcessStartInfo procInfo;
			Process proc;
			procInfo = new ProcessStartInfo("xprintidle");
			

			Process proc2 = new Process();
			proc2.StartInfo.FileName = "xprintidle";
			proc2.StartInfo.UseShellExecute = false;
			proc2.StartInfo.RedirectStandardOutput = true;
			//proc2.OutputDataReceived += HandleProcOutputDataReceived;
			//proc2.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			proc2.Start();
			string output = proc2.StandardOutput.ReadToEnd();
			proc2.WaitForExit();
			
			// Now actually take the screenshot
			int idleTime;
			string dirName = DateTime.Now.ToString ("MM-dd-yyyy");
			string fileName = DateTime.Now.ToString ("hhmmss");
			fileName += ".png";
			
			if(!Int32.TryParse (output.Split("\n".ToCharArray())[0], out idleTime))
			{
				// TODO: show an error now
				//Gtk.MessageDialog error = new Gtk.MessageDialog(null, null, MessageType.Error, ButtonsType.Ok, false, "xprintidle is missing");
				
				// gracefully exit
				Log.Error("Unable to parse xprintidle output");
				throw new InvalidDataException("Unable to parse xprintidle output");
			}
			
			Log.Debug ("Idletime is " + idleTime.ToString());
			
			if(idleTime > Preferences.Interval + 1000)
			{
				// ignore. idle
				Log.Debug ("Ignoring because of idleness");
				return;
			}
			
			
			if (Preferences.Enabled) {
				
				// create directory if it doesn't exist
				string fullDirPath = System.IO.Path.Combine(Preferences.SavePath, dirName);
				if (!Directory.Exists (fullDirPath)) {
					try {
						Directory.CreateDirectory (fullDirPath);
					} catch {
						// handle error
					}
				}
				string filePath = Path.Combine (dirName, fileName);
				
				// append the filepath to the save path in preferences
				filePath = Path.Combine (Preferences.SavePath, filePath);
					
				if (File.Exists (filePath)) {
					try {
						File.Delete (filePath);
					} catch {
						
					}
				}
				
				Log.Debug ("Saving to path - " + filePath);
				
				// Take the screenshot			
				int screenWidth = Gdk.Screen.Default.Width;
				int screenHeight = Gdk.Screen.Default.Height;
				
				Log.Debug(String.Format ("Scroting for {0}x{1}", screenWidth, screenHeight));
				
				Bitmap bmpScreenShot = new Bitmap (screenWidth, screenHeight);
				Graphics gfx = Graphics.FromImage ((System.Drawing.Image)bmpScreenShot);
				gfx.CopyFromScreen (0, 0, 0, 0, new Size (screenWidth, screenHeight));
				
				// Create a thumbnailed version of the screenshot
				int thumbWidth = (int)((double)screenWidth * ((double)Preferences.ScalePercentage / (double)100));
				int thumbHeight = (int)((double)screenHeight * ((double)Preferences.ScalePercentage / (double)100));
				
				Log.Debug ("Saving to disk");
				System.Drawing.Image thumb = bmpScreenShot.GetThumbnailImage (thumbWidth, thumbHeight, null, IntPtr.Zero);
				thumb.Save (filePath, ImageFormat.Png);
				
				// Clean up
				bmpScreenShot.Dispose ();
				gfx.Dispose ();
				thumb.Dispose ();
			}
			else {
				Log.Debug ("Screenlapse not enabled");
			}
		}
	}
}

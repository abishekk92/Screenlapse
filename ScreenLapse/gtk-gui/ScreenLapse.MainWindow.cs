
// This file has been generated by the GUI designer. Do not modify.
namespace ScreenLapse
{
	public partial class MainWindow
	{
		private global::Gtk.VButtonBox vbuttonbox1;

		private global::Gtk.Button button54;

		private global::Gtk.Button button55;

		private global::Gtk.Button button56;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget ScreenLapse.MainWindow
			this.Name = "ScreenLapse.MainWindow";
			this.Title = global::Mono.Unix.Catalog.GetString ("MainWindow");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Container child ScreenLapse.MainWindow.Gtk.Container+ContainerChild
			this.vbuttonbox1 = new global::Gtk.VButtonBox ();
			this.vbuttonbox1.Name = "vbuttonbox1";
			// Container child vbuttonbox1.Gtk.ButtonBox+ButtonBoxChild
			this.button54 = new global::Gtk.Button ();
			this.button54.CanFocus = true;
			this.button54.Name = "button54";
			this.button54.UseUnderline = true;
			// Container child button54.Gtk.Container+ContainerChild
			global::Gtk.Alignment w1 = new global::Gtk.Alignment (0.5f, 0.5f, 0f, 0f);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w2 = new global::Gtk.HBox ();
			w2.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w3 = new global::Gtk.Image ();
			w3.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-properties", global::Gtk.IconSize.Menu);
			w2.Add (w3);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w5 = new global::Gtk.Label ();
			w5.LabelProp = global::Mono.Unix.Catalog.GetString ("Preferences");
			w5.UseUnderline = true;
			w2.Add (w5);
			w1.Add (w2);
			this.button54.Add (w1);
			this.vbuttonbox1.Add (this.button54);
			global::Gtk.ButtonBox.ButtonBoxChild w9 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.vbuttonbox1[this.button54]));
			w9.Expand = false;
			w9.Fill = false;
			// Container child vbuttonbox1.Gtk.ButtonBox+ButtonBoxChild
			this.button55 = new global::Gtk.Button ();
			this.button55.CanFocus = true;
			this.button55.Name = "button55";
			this.button55.UseUnderline = true;
			// Container child button55.Gtk.Container+ContainerChild
			global::Gtk.Alignment w10 = new global::Gtk.Alignment (0.5f, 0.5f, 0f, 0f);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w11 = new global::Gtk.HBox ();
			w11.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w12 = new global::Gtk.Image ();
			w12.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-media-play", global::Gtk.IconSize.Menu);
			w11.Add (w12);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w14 = new global::Gtk.Label ();
			w14.LabelProp = global::Mono.Unix.Catalog.GetString ("Playback");
			w14.UseUnderline = true;
			w11.Add (w14);
			w10.Add (w11);
			this.button55.Add (w10);
			this.vbuttonbox1.Add (this.button55);
			global::Gtk.ButtonBox.ButtonBoxChild w18 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.vbuttonbox1[this.button55]));
			w18.Position = 1;
			w18.Expand = false;
			w18.Fill = false;
			// Container child vbuttonbox1.Gtk.ButtonBox+ButtonBoxChild
			this.button56 = new global::Gtk.Button ();
			this.button56.CanFocus = true;
			this.button56.Name = "button56";
			this.button56.UseUnderline = true;
			this.button56.Label = global::Mono.Unix.Catalog.GetString ("GtkButton");
			this.vbuttonbox1.Add (this.button56);
			global::Gtk.ButtonBox.ButtonBoxChild w19 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.vbuttonbox1[this.button56]));
			w19.Position = 2;
			w19.Expand = false;
			w19.Fill = false;
			this.Add (this.vbuttonbox1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 400;
			this.DefaultHeight = 300;
			this.Show ();
			this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
			this.button54.Clicked += new global::System.EventHandler (this.ShowPreferencesWindow);
			this.button55.Clicked += new global::System.EventHandler (this.OnButton55Clicked);
		}
	}
}

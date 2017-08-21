/*
 * Filename: iBar.cs
 * Author: Marcus Grenängen (marcus@grenangen.se)
 * Create Date: 2007-02-26
 * 
 * Comment(s): 
 *		This class / Control uses the same look-n-feel as the iTunes bar for the iPod aviable space
 * 
 * 2007-02-27: Added new type to the control. Animation
 *			   This type "Rubber bands" the procent bar to the new value.
 *			   This idéa was submitted by: Ben_64 @ codeproject. Tanks
 * 
 * 2007-12-31: Added System.ComponentModel.Description and System.ComponentModel.DefaultValue + System.ComponentModel.RefreshProperties
 *			   to the control properties to make the control properties more user friendly.
 *			   Changed the OnBarValueChanged event implementation to be declared in a more readable and 
 *			   understandable way. 
 *			   Added System.Drawing.ToolboxBitmap to the class implementation to support the Toolbox in a better way
 *			   These changes was suggested by chris175 @ codeproject. Tanks
 */
using System;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.ComponentModel;

namespace Sheng.SailingEase.Controls
{
	/// <summary>
	/// This enum represents the diffrent states the control can take.
	/// Animation for ex. makes the procent bar "Rubber band" to the new procent value
	/// </summary>
	public enum BarType
	{
		Static,			// Plain and simple bar with procent display
		Progressbar,	// This makes the control act as a progressbar
		Animated		// This makes the control "Rubber band" to new procent values (Animated).
	};

	/// <summary>
	/// A delegate to be able to trigger value changed event
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void BarValueChanged( object sender, EventArgs e );

	/// <summary>
	/// A iTunes look-a-like bar for displaying various data.
	/// </summary>
	[System.Drawing.ToolboxBitmap( typeof( System.Windows.Forms.ProgressBar ) )]
    [LicenseProvider(typeof(SEControlLicenseProvider))]
	public partial class SEiBar : UserControl
	{
		#region Static Members
		public static readonly Color PreBarBaseDark		= Color.FromArgb( 199, 200, 201 );
		public static readonly Color PreBarBaseLight	= Color.WhiteSmoke;
		public static readonly Color PreBarLight		= Color.FromArgb( 102, 144, 252 );
		public static readonly Color PreBarDark			= Color.FromArgb( 40, 68, 202 );
		public static readonly Color PreBorderColor		= Color.DarkGray;
		#endregion

		#region Private Members
		/// <summary>
		/// The Light background color
		/// </summary>
		private Color clrBarBgLight = PreBarBaseLight;
		
		/// <summary>
		/// The Dark background color
		/// </summary>
		private Color clrBarBgDark = PreBarBaseDark;
		
		/// <summary>
		/// The Light bar color
		/// </summary>
		private Color clrBarLight = PreBarLight;

		/// <summary>
		/// The Dark bar color
		/// </summary>
		private Color clrBarDark = PreBarDark;

		/// <summary>
		/// The Border color for the control
		/// </summary>
		private Color clrBorderColor = PreBorderColor;

		/// <summary>
		/// The border width
		/// </summary>
		private float fBorderWidth = 1.25F;

		/// <summary>
		/// This is the % value that describs the amount of transparency 
		/// through the background that we will show.
		/// </summary>
		private float fMirrorOpacity = 40.0F;

		/// <summary>
		/// This is the % value that will be filled in the bar
		/// </summary>
		private float fFillProcent = 50.0F;

		/// <summary>
		/// The number of dividers to draw in the bar
		/// </summary>
		private int iNumDividers = 10;

		/// <summary>
		/// A timer to enable progressbar feature
		/// </summary>
		private Timer tTickTimer = new Timer();

		/// <summary>
		/// The stepsize to use when the control is a progressbar
		/// </summary>
		private float iStepSize = 0;

		/// <summary>
		/// A member to control the way the bar behaves
		/// </summary>
		private BarType eBarType = BarType.Static;

		/// <summary>
		/// The timer to handle the progress tick event
		/// </summary>
		private Timer tAnimTicker = new Timer();

		/// <summary>
		/// A value to save the new target procent when we are in Animation mode
		/// </summary>
		private float fNewProcent = 0.0F;

		/// <summary>
		/// A simple flag to indicate what way the control should animate it's progress when it's in Animation mode
		/// </summary>
		private bool bAnimUp = false;
		#endregion

		#region Events
		/// <summary>
		/// This event is to notify the user that the progress of the control has changed
		/// </summary>
		//public event BarValueChanged OnBarValueChanged = null;
		public event EventHandler<EventArgs> OnBarValueChanged = null;
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="iBar"/> class.
		/// </summary>
		public SEiBar()
		{
            LicenseManager.Validate(typeof(SEiBar)); 

			SetStyle( ControlStyles.OptimizedDoubleBuffer | 
					  ControlStyles.AllPaintingInWmPaint | 
					  ControlStyles.UserPaint | 
					  ControlStyles.ResizeRedraw, true );

			InitializeComponent();
			this.Width = 300;
			this.Height = 50;

			// Progressbar timer
			this.tTickTimer.Tick += new EventHandler( TickTimer_Tick );
			this.tTickTimer.Enabled = false;
			this.tTickTimer.Interval = 100;

			// Animation timer
			tAnimTicker.Enabled = false;
			tAnimTicker.Interval = 75;
			tAnimTicker.Tick += new EventHandler( AnimationTimer_Tick );

		} // END CONSTRUCTOR: iBar( ... )

		#region Public Methods
		/// <summary>
		/// Returns the control rendered to a Bitmap.
		/// This method can be used in Web environment.
		/// </summary>
		/// <returns></returns>
		public Bitmap ToImage()
		{
			Bitmap retVal = new Bitmap( this.Width, this.Height );
			Graphics g = Graphics.FromImage( retVal );
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.Clear( this.BackColor );

			Bitmap bmp = GenerateProcentBar( this.Width, this.Height, this.fFillProcent, this.fMirrorOpacity, this.BackColor );
			g.DrawImage( bmp, 0, 0 );

			g.Dispose();
			return retVal;

		} // END METHOD: ToImage( ... )
		#endregion

		/// <summary>
		/// Handles the Tick event of the TickTimer control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void TickTimer_Tick( object sender, EventArgs e )
		{
			this.fFillProcent += this.iStepSize;
			if( this.fFillProcent >= 100.0F )
				this.fFillProcent = 0.0F;
			else if( this.fFillProcent < 0.0F )
				this.fFillProcent = 100.0F;

			Refresh();

			// Trigger event if any defined
			if( OnBarValueChanged != null )
				OnBarValueChanged( this, EventArgs.Empty );

		} // END METHOD: TickTimer_Tick( ... )

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint( PaintEventArgs e )
		{
			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.Clear( this.BackColor );

			Bitmap bmp = GenerateProcentBar( this.Width, this.Height, this.fFillProcent, this.fMirrorOpacity, this.BackColor );
			g.DrawImage( bmp, 0, 0 );

		} // END METHOD: OnPaint( ... )

		/// <summary>
		/// Generates the bar.
		/// </summary>
		/// <param name="g">The g.</param>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="fillProcent">The fill procent.</param>
		private void GenerateBar( ref Graphics g, float x, float y, 
								  float width, float height, float fillProcent )
		{
			float procentMarkerWidth = ( width / iNumDividers );
			float totalWidth = width;
			RectangleF rect = new RectangleF( x, y, width, height );

			using( LinearGradientBrush white = new LinearGradientBrush( rect, clrBarBgLight, clrBarBgDark, 90.0F, false ) )
			{
				g.FillRectangle( white, rect );

			} // END using( LinearGradientBrush white = new LinearGradientBrush( rect, clrBarBgLight, clrBarBgDark, 90.0F, false ) )

			using( Pen p = new Pen( this.clrBorderColor, this.fBorderWidth * 2 ) )
			{
				p.Alignment = PenAlignment.Outset;
				p.LineJoin = LineJoin.Round;
				g.DrawRectangle( p, x, y, width, height );

			} // END using( Pen p = new Pen( this.clrBorderColor, this.fBorderWidth * 2 ) )

			width = ( fillProcent > 0 ? ( ( fillProcent / 100 ) * width ) : 0 );
			if( width > 0.10F )
			{
				rect = new RectangleF( x, y, width, height );
				using( LinearGradientBrush bg = new LinearGradientBrush( rect, this.clrBarLight, this.clrBarDark, 90.0F, false ) )
				{
					g.FillRectangle( bg, rect );

				} // END using( LinearGradientBrush bg = new LinearGradientBrush( rect, this.clrBarLight, this.clrBarDark, 90.0F, false ) )

				using( Pen p = new Pen( this.clrBorderColor, this.fBorderWidth ) )
				{
					p.Alignment = PenAlignment.Inset;
					p.LineJoin = LineJoin.Round;
					g.DrawLine( p, width, y, width, height );

				} // END using( Pen p = new Pen( this.clrBorderColor, this.fBorderWidth ) )

			} // END if( width > 0 )

			using( Pen p = new Pen( this.clrBarDark, this.fBorderWidth ) )
			{
				p.Alignment = PenAlignment.Inset;
				p.LineJoin = LineJoin.Round;
				using( Pen p2 = new Pen( this.clrBarLight, this.fBorderWidth ) )
				{
					p2.Alignment = PenAlignment.Inset;
					p2.LineJoin = LineJoin.Round;
					for( float i = procentMarkerWidth; i < totalWidth; i += procentMarkerWidth )
					{
						if( i >= width )
						{
							p.Color = clrBarBgLight;
							p2.Color = clrBarBgDark;

						} // END if( i >= width )

						g.DrawLine( p, i, 0, i, height );
						g.DrawLine( p2, i + this.fBorderWidth, 0, i + this.fBorderWidth, height );

					} // END for( float i = procentMarkerWidth; i < totalWidth; i += procentMarkerWidth )

				} // END using( Pen p2 = new Pen( this.clrBarLight, this.fBorderWidth ) )

			} // END using( Pen p = new Pen( this.clrBarDark, this.fBorderWidth ) )
		
		} // END METHOD: GenerateBar( ... )

		/// <summary>
		/// Generates the bar image.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="procent">The procent.</param>
		/// <returns></returns>
		private Bitmap GenerateBarImage( int width, int height, float procent )
		{
			Bitmap bmp = new Bitmap( width, height );
			Graphics g = Graphics.FromImage( bmp );

			GenerateBar( ref g, 0.0F, 0.0F, width, height, procent );
			g.Dispose();

			return bmp;
		
		} // END METHOD: GenerateBarImage( ... )

		/// <summary>
		/// Makes the colors fade to the background.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="bgColor">Color of the bg.</param>
		/// <param name="angle">The angle.</param>
		/// <returns></returns>
		private Bitmap FadeToBg( Bitmap image, Color bgColor, float angle )
		{
			Bitmap bmp = new Bitmap( image.Width, image.Height );
			Graphics g = Graphics.FromImage( bmp );

			g.DrawImage( image, 0, 0 );
			Rectangle source = new Rectangle( 0, -1, bmp.Width, bmp.Height );
			using( LinearGradientBrush bg = new LinearGradientBrush( source, Color.Transparent, bgColor, angle, false ) )
			{
				g.FillRectangle( bg, source );

			} // END using( LinearGradientBrush bg = new LinearGradientBrush( source, Color.Transparent, bgColor, angle, false ) )

			g.Dispose();

			return bmp;
		
		} // END METHOD: FadeToBg( ... )

		/// <summary>
		/// Generates the procent bar.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="procent">The procent.</param>
		/// <param name="mirrorOpacity">The mirror opacity.</param>
		/// <returns></returns>
		private Bitmap GenerateProcentBar( int width, int height, float procent, float mirrorOpacity, Color bgColor )
		{
			Bitmap theImage = new Bitmap( width, height );
			Graphics g = Graphics.FromImage( theImage );

			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.Clear( bgColor );

			Bitmap bmp = GenerateBarImage( width, height - ( height / 3 ), procent );
			Bitmap mirror = FadeToBg( bmp, bgColor, -90.0F );
			GraphicsState state = g.Save();
			g.ScaleTransform( 1, -1.0F, MatrixOrder.Prepend );

			ColorMatrix clr = new ColorMatrix();
			ImageAttributes attributes = new ImageAttributes();

			clr.Matrix33 = ( mirrorOpacity / 100 );
			attributes.SetColorMatrix( clr );

			Rectangle source = new Rectangle( 0, -( height ), mirror.Width, mirror.Height );
			g.DrawImage( mirror, source, 0, 0, mirror.Width, mirror.Height, GraphicsUnit.Pixel, attributes );

			g.Restore( state );
			g.DrawImage( bmp, 0, -5 );

			g.Dispose();
			bmp.Dispose();
			mirror.Dispose();

			return theImage;
		
		} // END METHOD: GenerateProcentBar( ... )

		/// <summary>
		/// Makes the animation.
		/// </summary>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		private void MakeAnimation( float newProcent )
		{
			fNewProcent = newProcent;
			bAnimUp = ( ( fFillProcent - newProcent ) > 0 );
			tAnimTicker.Interval = 1;
			tAnimTicker.Enabled = true;

		} // END METHOD: MakeAnimation( ... )

		/// <summary>
		/// Handles the Tick event of the AnimationTimer control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void AnimationTimer_Tick( object sender, EventArgs e )
		{
			fFillProcent -= ( ( fFillProcent - fNewProcent ) / 5.0F );
			tAnimTicker.Enabled = ( !bAnimUp ? ( fFillProcent <= fNewProcent ) : ( fFillProcent >= fNewProcent ) );

			Refresh();

		} // END METHOD: AnimationTimer_Tick( ... )


		#region Access Methods
		/// <summary>
		/// Gets or sets the size of the step.
		/// When this value is > 0 the progressbar is enabled
		/// </summary>
		/// <value>The size of the step.</value>
		[System.ComponentModel.Description( "Gets or sets the stepsize. This controls how many steps it will progress when making a move" )]
		[System.ComponentModel.DefaultValue( 2 )]
		public float StepSize
		{
			get { return iStepSize; }
			set
			{ 
				iStepSize = value;
				this.tTickTimer.Enabled = ( this.iStepSize != 0 && this.tTickTimer.Interval > 0 && eBarType == BarType.Progressbar );
			}
		}

		/// <summary>
		/// Gets or sets the step interval.
		/// This is the interval that determins how often the control 
		/// is updated with it's step size
		/// </summary>
		/// <value>The step interval.</value>
		[System.ComponentModel.Description( "Gets or sets the StepInterval. This value determins how ofthen the control is updated using the StepSize" )]
		[System.ComponentModel.DefaultValue( 5 )]
		public int StepInterval
		{
			get { return tTickTimer.Interval; }
			set
			{
				tTickTimer.Interval = value;
				this.tTickTimer.Enabled = ( this.iStepSize != 0 && this.tTickTimer.Interval > 0 && eBarType == BarType.Progressbar );
			}
		}

		/// <summary>
		/// Gets or sets the number of bar dividers to display.
		/// </summary>
		/// <value>The num bar dividers.</value>
		[System.ComponentModel.Description( "Gets or sets how many dividers to display on the bar" )]
		[System.ComponentModel.DefaultValue( 10 )]
		[System.ComponentModel.RefreshProperties( System.ComponentModel.RefreshProperties.Repaint )]
		public int BarDividersCount
		{
			get { return iNumDividers; }
			set { iNumDividers = value; Refresh(); }
		}

		/// <summary>
		/// Gets or sets the mirror opacity.
		/// </summary>
		/// <value>The mirror opacity.</value>
		[System.ComponentModel.Description( "Gets or sets the opacity level for the reflecting part of the control" )]
		[System.ComponentModel.DefaultValue( 35.0f )]
		[System.ComponentModel.RefreshProperties( System.ComponentModel.RefreshProperties.Repaint )]
		public float BarMirrorOpacity
		{
			get { return fMirrorOpacity; }
			set { fMirrorOpacity = value; Refresh(); }
		}

		/// <summary>
		/// Gets or sets the fill procent.
		/// </summary>
		/// <value>The fill procent.</value>
		[System.ComponentModel.Description( "Gets or sets the fill procent" )]
		[System.ComponentModel.DefaultValue( 50.0f )]
		[System.ComponentModel.RefreshProperties( System.ComponentModel.RefreshProperties.Repaint )]
		public float BarFillProcent
		{
			get { return fFillProcent; }
			set
			{
				if( this.eBarType == BarType.Animated )
				{
					MakeAnimation( value );
					return;
				}
				
				fFillProcent = value;
				Refresh();
			}
		}

		/// <summary>
		/// Gets or sets the width of the border.
		/// </summary>
		/// <value>The width of the border.</value>
		[System.ComponentModel.Description( "Gets or sets the with of the borders" )]
		[System.ComponentModel.DefaultValue( 1.0f )]
		[System.ComponentModel.RefreshProperties( System.ComponentModel.RefreshProperties.Repaint )]
		public float BarBorderWidth
		{
			get { return fBorderWidth; }
			set { fBorderWidth = value; Refresh(); }
		}

		/// <summary>
		/// Gets or sets the bar background light.
		/// </summary>
		/// <value>The bar background light.</value>
		[System.ComponentModel.Description( "Gets or sets the lighter background color" )]
		[System.ComponentModel.RefreshProperties( System.ComponentModel.RefreshProperties.Repaint )]
		public Color BarBackgroundLight
		{
			get { return clrBarBgLight; }
			set { clrBarBgLight = value; Refresh(); }
		}

		/// <summary>
		/// Gets or sets the bar background dark.
		/// </summary>
		/// <value>The bar background dark.</value>
		[System.ComponentModel.Description( "Gets or sets the darker background color" )]
		[System.ComponentModel.RefreshProperties( System.ComponentModel.RefreshProperties.Repaint )]
		public Color BarBackgroundDark
		{
			get { return clrBarBgDark; }
			set { clrBarBgDark = value; Refresh(); }
		}

		/// <summary>
		/// Gets or sets the bar light.
		/// </summary>
		/// <value>The bar light.</value>
		[System.ComponentModel.Description( "Gets or sets the light bar color" )]
		[System.ComponentModel.RefreshProperties( System.ComponentModel.RefreshProperties.Repaint )]
		public Color BarLight
		{
			get { return clrBarLight; }
			set { clrBarLight = value; Refresh(); }
		}

		/// <summary>
		/// Gets or sets the bar dark.
		/// </summary>
		/// <value>The bar dark.</value>
		[System.ComponentModel.Description( "Gets or sets the dark bar color" )]
		[System.ComponentModel.RefreshProperties( System.ComponentModel.RefreshProperties.Repaint )]
		public Color BarDark
		{
			get { return clrBarDark; }
			set { clrBarDark = value; Refresh(); }
		}

		/// <summary>
		/// Gets or sets the color of the border.
		/// </summary>
		/// <value>The color of the border.</value>
		[System.ComponentModel.Description( "Gets or sets the border color" )]
		[System.ComponentModel.RefreshProperties( System.ComponentModel.RefreshProperties.Repaint )]
		public Color BarBorderColor
		{
			get { return clrBorderColor; }
			set { clrBorderColor = value; Refresh(); }
		}

		/// <summary>
		/// Gets or sets the type of the bar.
		/// </summary>
		/// <value>The type of the bar.</value>
		[System.ComponentModel.Description( "Gets or sets the type. This changes the bahaviour of the control. See the BarType enum for specification" )]
		[System.ComponentModel.DefaultValue( BarType.Animated )]
		[System.ComponentModel.RefreshProperties( System.ComponentModel.RefreshProperties.Repaint )]
		public BarType BarType
		{
			get { return eBarType; }
			set
			{
				eBarType = value;
				if( value != BarType.Progressbar )
				{
					this.iStepSize = 0;
					this.tTickTimer.Enabled = false;
				}
			}
		}
		#endregion

	} // END CLASS: iBar

} // END NAMESPACE: MG.Controls.BarLib
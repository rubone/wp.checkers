using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using checkers.Resources;
using System.Windows.Shapes;
using System.Windows.Media;

namespace checkers
{

    public class Piece
    {
        public string name;
        public int row;
        public int column;
        public bool isPlayerOne;

        public Piece(string _name, int _row, int _column, bool _isPlayerOne) {
            this.name = _name;
            this.row = _row;
            this.column = _column;
            this.isPlayerOne = _isPlayerOne;
        }
    }

    public partial class MainPage : PhoneApplicationPage
    {

        private Dictionary<String,  CompositeTransform> transformation = new Dictionary<String, CompositeTransform>();
        private List<Piece> boardPiece = new List<Piece>();
        private bool move;
        private bool player;
        private bool player1Blocked;
        private bool player2Blocked;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
            this.LoadDictionaryPiece();    
            // Inicializando Variables de control
            this.move = false;
            this.player = true;  //true--> Jugador #1 false --> Jugador #2
            this.player1Blocked = false;
            this.player2Blocked = true;

        }        

        #region Initial Methods

        /// <summary>
        /// Metodo para agregar las fichas para poder controlar sus movimientos
        /// </summary>
        private void LoadDictionaryPiece()
        {
            try
            {
               
                Piece playerPiece;
                boardPiece.Clear();

                #region Configurando Jugador #1
                //Fichas Jugador #1
                playerPiece = new Piece("piece1", 0, 1, true);
                transformation.Add("piece1", transform_piece1);
                boardPiece.Add(playerPiece);
                //--
                playerPiece = new Piece("piece2", 0, 3, true);
                transformation.Add("piece2", transform_piece2);
                boardPiece.Add(playerPiece);
                //--
                playerPiece = new Piece("piece3", 0, 5, true);
                transformation.Add("piece3", transform_piece3);
                boardPiece.Add(playerPiece);
                //--
                playerPiece = new Piece("piece4", 0, 7, true);
                transformation.Add("piece4", transform_piece4);
                boardPiece.Add(playerPiece);
                //--
                playerPiece = new Piece("piece5", 1, 0, true);
                transformation.Add("piece5", transform_piece5);
                boardPiece.Add(playerPiece);
                //--
                playerPiece = new Piece("piece6", 1, 2, true);
                transformation.Add("piece6", transform_piece6);
                boardPiece.Add(playerPiece);
                //--
                playerPiece = new Piece("piece7", 1, 4, true);
                transformation.Add("piece7", transform_piece7);
                boardPiece.Add(playerPiece);
                //--
                playerPiece = new Piece("piece8", 1, 6, true);
                transformation.Add("piece8", transform_piece8);
                boardPiece.Add(playerPiece);
                //--
                playerPiece = new Piece("piece9", 2, 1, true);
                transformation.Add("piece9", transform_piece9);
                boardPiece.Add(playerPiece);
                //--
                playerPiece = new Piece("piece10", 2, 3, true);
                transformation.Add("piece10", transform_piece10);
                boardPiece.Add(playerPiece);
                //--
                playerPiece = new Piece("piece11", 2, 5, true);
                transformation.Add("piece11", transform_piece11);
                boardPiece.Add(playerPiece);
                //--
                playerPiece = new Piece("piece12", 2, 7, true);
                transformation.Add("piece12", transform_piece12);
                boardPiece.Add(playerPiece);
                #endregion

                #region Configurando Jugador #2
                //Fichas Jugador #2                
                playerPiece = new Piece("piece13", 5, 0, false);
                transformation.Add("piece13", transform_piece13);
                boardPiece.Add(playerPiece);
                //--
                playerPiece = new Piece("piece14", 5, 2, false);
                transformation.Add("piece14", transform_piece14);
                boardPiece.Add(playerPiece);
                //--
                playerPiece = new Piece("piece15", 5, 4, false);
                transformation.Add("piece15", transform_piece15);
                boardPiece.Add(playerPiece);
                //--
                playerPiece = new Piece("piece16", 5, 6, false);
                transformation.Add("piece16", transform_piece16);
                boardPiece.Add(playerPiece);
                //--
                playerPiece = new Piece("piece17", 6, 1, false);
                transformation.Add("piece17", transform_piece17);
                boardPiece.Add(playerPiece);
                //--
                playerPiece = new Piece("piece18", 6, 3, false);
                transformation.Add("piece18", transform_piece18);
                boardPiece.Add(playerPiece);
                //--
                playerPiece = new Piece("piece19", 6, 5, false);
                transformation.Add("piece19", transform_piece19);
                boardPiece.Add(playerPiece);
                //--
                playerPiece = new Piece("piece20", 6, 7, false);
                transformation.Add("piece20", transform_piece20);
                boardPiece.Add(playerPiece);
                //--
                playerPiece = new Piece("piece21", 7, 0, false);
                transformation.Add("piece21", transform_piece21);
                boardPiece.Add(playerPiece);
                //--
                playerPiece = new Piece("piece23", 7, 3, false);
                transformation.Add("piece22", transform_piece22);
                boardPiece.Add(playerPiece);
                //--
                playerPiece = new Piece("piece23", 7, 5, false);
                transformation.Add("piece23", transform_piece23);
                boardPiece.Add(playerPiece);
                //--
                playerPiece = new Piece("piece24", 7, 7, false);
                transformation.Add("piece24", transform_piece24);
                boardPiece.Add(playerPiece);

                #endregion

            }
            catch (Exception ex)
            {
                ex.GetType();
            }
        }

        #endregion

        #region Validate Methods

        private bool isValidMove(int sourceRow, int sourceColumn, int newRow, int newColumn) {
            bool isvalid = false;           
                isvalid = true;
                if (newRow == sourceRow) {
                    isvalid = false;
                    return isvalid;
                }
                int absrow = Math.Abs(sourceRow - newRow);
                int abscolumn = Math.Abs(sourceColumn - newColumn);
                return absrow == abscolumn;            
        }

        private int ConvertirEscala(double scale50)
        {
            if (scale50 > 0)//si se navega de arriba hacia abajo (player2)
            { 
                return (int)Math.Floor(scale50 / 50);
            }
            return (int)Math.Ceiling(scale50 / 50); //si se navega de abajo hacia arriba (player1)
        }

        #endregion


        private void Ellipse_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            Ellipse objectPiece = (sender as Ellipse);
            Piece selectPiece = (from p in boardPiece where p.name.Equals(objectPiece.GetValue(NameProperty)) select p).Single();
            if (!this.player && selectPiece.isPlayerOne || this.player && selectPiece.isPlayerOne == false)
            {
                this.message.Text = "No es tu turno";
                this.move = false;
            }else{
                this.move = true;
                this.message.Text = "";
            }
        }

        void Ellipse_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            try
            {
                if (this.move)
                {
                    Ellipse objectPiece = (sender as Ellipse);
                    objectPiece.RenderTransform = transformation[objectPiece.Name];
                    transformation[objectPiece.Name].TranslateX += e.DeltaManipulation.Translation.X;
                    transformation[objectPiece.Name].TranslateY += e.DeltaManipulation.Translation.Y;                    
                }
            }
            catch (Exception ex) {
                ex.GetType();
            }
        }

        private void Ellipse_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            try {
                Ellipse objectPiece = (sender as Ellipse);
                Piece selectPiece = (from p in boardPiece where p.name.Equals(objectPiece.GetValue(NameProperty)) select p).Single();
                if (this.move)
                {
                    
                    int finalrow = Grid.GetRow(objectPiece) + this.ConvertirEscala(transformation[objectPiece.Name].TranslateY);
                    int finalcolumn = Grid.GetColumn(objectPiece) + this.ConvertirEscala(transformation[objectPiece.Name].TranslateX);
                    if (this.isValidMove(selectPiece.row, selectPiece.column, finalrow, finalcolumn)) 
                    {
                        selectPiece.row = finalrow;
                        selectPiece.column = finalcolumn;
                        Grid.SetRow(objectPiece, finalrow);
                        Grid.SetColumn(objectPiece, finalcolumn);
                        this.player = !this.player;
                    }
                    transformation[objectPiece.Name].TranslateX = 0;
                    transformation[objectPiece.Name].TranslateY = 0;
                }
            }
            catch (Exception ex) {
                ex.GetType();
            }
        }


        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}
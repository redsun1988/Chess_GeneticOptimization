using System;
using System.Drawing;
using System.Windows.Forms;
using ChessComponent.Main_Classes.Control;
using ChessComponent.Main_Classes.Model.Service_Class;

namespace ChessComponent
{
    public partial class ChessBordView : UserControl, IUserObserver
    {
        public ChessBordView()
        {
            InitializeComponent();

            _chessControl1.Bind(this);

            _setDataSafeHandler = new SetDataSafeDelegate(SetDataSafe);

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            _g = Graphics.FromImage(pictureBox1.Image);
        }

        private void UpdateNumericUpDowns()
        {
            numericUpDown1.Value = ChessBorderSize;
            numericUpDown2.Value = IndividualCount;
        }

        public int ChessBorderSize
        {
            get
            {
                return _chessControl1.ChessBoardSize;
            }

            set
            {
                _chessControl1.ChessBoardSize = value;
                UpdateNumericUpDowns();
            }
        }

        public int IndividualCount
        {
            get
            {
                return _chessControl1.IndividualCount;
            }
            set
            {
                _chessControl1.IndividualCount = value;
                UpdateNumericUpDowns();
            }
        }
   
        public OptimGoal OptGoal
        {
            get
            {
                return _chessControl1.OptGoal;
            }
            set
            {
                _chessControl1.OptGoal = value;             
            }
        }

        public void Next(int iterationCount)
        {
            _chessControl1.Next(iterationCount);
        }

        public void Update(Issue outData)
        {
            SetDataSafe(outData);
            DrowChessBoard(outData.Lider);
        }

        private readonly Graphics _g;

        private void SetDataSafe(Issue outData)
        {
            SetProgressBarSafe(outData);
            SetLabeSafe(outData);
        }

        private void SetLabeSafe(Issue outData)
        {
            if (label5.InvokeRequired)
            {
                label5.Invoke(_setDataSafeHandler, new object[] { outData });
            }
            else
            {
                label5.Text = "The best result is " + Convert.ToString(outData.Lider.Goal);
            }
        }

        private void SetProgressBarSafe(Issue outData)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(_setDataSafeHandler, new object[] { outData });
            }
            else
            {
                progressBar1.Maximum = outData.CountOfIterations;
                progressBar1.Value = outData.Interation;
            }
        }

        private delegate void SetDataSafeDelegate(Issue outData);
        private readonly SetDataSafeDelegate _setDataSafeHandler;

        private readonly ChessControl _chessControl1 = new ChessControl();

        private void DrowChessBoard(Individual item)
        {
            Brush currentBrash;
            Image horse = Properties.Resources.horse;

            for (byte i = 0; i <= (item.Row - 1); i++)
            {
                for (byte j = 0; j <= (item.Col - 1); j++)
                {
                    if ((i + j) % 2 == 0)
                        currentBrash = new SolidBrush(Color.Black);
                    else
                        currentBrash = new SolidBrush(Color.White);

                    var currentRec = new RectangleF(i * (pictureBox1.Size.Width / (float)item.Col),
                            j * (pictureBox1.Size.Height / (float)item.Row),
                            (pictureBox1.Size.Width / (float)item.Col),
                            (pictureBox1.Size.Height / (float)item.Row));

                    _g.FillRectangle(currentBrash,
                        currentRec);

                    if (item[i, j] != 0)
                        _g.DrawImage(horse, currentRec);           
                }
            }

            pictureBox1.Invalidate();
        }

        private void Button1Click(object sender, EventArgs e)
        {
            Next((int)numericUpDown3.Value);
        }

        private void NumericUpDown1ValueChanged(object sender, EventArgs e)
        {
            ChessBorderSize = (int)numericUpDown1.Value;
        }

        private void NumericUpDown2ValueChanged(object sender, EventArgs e)
        {
            IndividualCount = (int)numericUpDown2.Value;

        }

    }
}

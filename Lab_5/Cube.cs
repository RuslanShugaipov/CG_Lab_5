using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Lab_5
{
    class Cube
    {
        public Cube(float side, int[] centerPoint)
        {
            this.side = side;
            this.centerPoint = new Matrix(new float[1, 4] { { centerPoint[0], centerPoint[1], centerPoint[2], 1 } });
            vertecies = new Matrix[]
            {
                new Matrix(new float[1, 4] { { this.centerPoint[0, 0] - side / 2, this.centerPoint[0, 1] + side / 2, this.centerPoint[0, 2] + side / 2, this.centerPoint[0, 3] } }),
                new Matrix(new float[1, 4] { { this.centerPoint[0, 0] + side / 2, this.centerPoint[0, 1] + side / 2, this.centerPoint[0, 2] + side / 2, this.centerPoint[0, 3] } }),
                new Matrix(new float[1, 4] { { this.centerPoint[0, 0] + side / 2, this.centerPoint[0, 1] - side / 2, this.centerPoint[0, 2] + side / 2, this.centerPoint[0, 3] } }),
                new Matrix(new float[1, 4] { { this.centerPoint[0, 0] - side / 2, this.centerPoint[0, 1] - side / 2, this.centerPoint[0, 2] + side / 2, this.centerPoint[0, 3] } }),
                new Matrix(new float[1, 4] { { this.centerPoint[0, 0] - side / 2, this.centerPoint[0, 1] + side / 2, this.centerPoint[0, 2] - side / 2, this.centerPoint[0, 3] } }),
                new Matrix(new float[1, 4] { { this.centerPoint[0, 0] + side / 2, this.centerPoint[0, 1] + side / 2, this.centerPoint[0, 2] - side / 2, this.centerPoint[0, 3] } }),
                new Matrix(new float[1, 4] { { this.centerPoint[0, 0] + side / 2, this.centerPoint[0, 1] - side / 2, this.centerPoint[0, 2] - side / 2, this.centerPoint[0, 3] } }),
                new Matrix(new float[1, 4] { { this.centerPoint[0, 0] - side / 2, this.centerPoint[0, 1] - side / 2, this.centerPoint[0, 2] - side / 2, this.centerPoint[0, 3] } })
            };
            V = new Matrix(4, 6);
            checkFaces();
        }

        private void checkFaces()
        {
            Matrix C = new Matrix(3, 1);
            Matrix X = new Matrix(new float[3, 3]);
            for (int i = 0; i < 6; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    X[0, j] = vertecies[indicies2[i, 0]][0, j];
                    X[1, j] = vertecies[indicies2[i, 1]][0, j];
                    X[2, j] = vertecies[indicies2[i, 2]][0, j];
                }
                C = coeffs(X);
                V[0, i] = C[0, 0];
                V[1, i] = C[1, 0];
                V[2, i] = C[2, 0];
                V[3, i] = -1;
            }
        }

        public void Draw(Graphics graphics, Pen pen)
        {
            checkFaces();
            Matrix E = new Matrix(new float[1, 4] { { 0, 0, -1, 0 } });
            Matrix EV = E * V;
            for (int j = 0; j < 6; ++j)
            {
                if (EV[0, j] < 0)
                    continue;
                GraphicsPath graphPath = new GraphicsPath();
                for (int i = 0; i < 7; ++i)
                {
                    graphPath.AddLine(vertecies[indicies[j, i]][0, 0], vertecies[indicies[j, i]][0, 1],
                        vertecies[indicies[j, i + 1]][0, 0], vertecies[indicies[j, i + 1]][0, 1]);
                }
                graphics.FillPath(brushes[j], graphPath);
                graphPath.Dispose();
            }
        }

        public void rotationX(double angle)
        {
            angle = angle * (Math.PI / 180);
            Matrix rotationX = new Matrix(new float[4, 4]{
                {    1,                         0,                         0, 0 },
                {    0,    (float)Math.Cos(angle),    (float)Math.Sin(angle), 0 },
                {    0, -((float)Math.Sin(angle)),    (float)Math.Cos(angle), 0 },
                {    0,                         0,                         0, 1 }
            });
            for (int i = 0; i < 8; ++i)
            {
                vertecies[i] = vertecies[i] * rotationX;
            }
        }

        public void rotationY(double angle)
        {
            angle = angle * (Math.PI / 180);
            Matrix rotationY = new Matrix(new float[4, 4]{
                {    (float)Math.Cos(angle), 0, -((float)Math.Sin(angle)), 0 },
                {                         0, 1,                         0, 0 },
                {    (float)Math.Sin(angle), 0,    (float)Math.Cos(angle), 0 },
                {                         0, 0,                         0, 1 }
            });
            for (int i = 0; i < 8; ++i)
            {
                vertecies[i] = vertecies[i] * rotationY;
            }
        }

        public void rotationZ(double angle)
        {
            angle = angle * (Math.PI / 180);
            Matrix rotationZ = new Matrix(new float[4, 4]{
                {    (float)Math.Cos(angle), (float)Math.Sin(angle), 0, 0 },
                { -((float)Math.Sin(angle)), (float)Math.Cos(angle), 0, 0 },
                {                         0,                      0, 1, 0 },
                {                         0,                      0, 0, 1 }
            });
            for (int i = 0; i < 8; ++i)
            {
                vertecies[i] = vertecies[i] * rotationZ;
            }
        }

        private int[,] indicies =
        {
            //Front face
            {0, 1, 1, 2, 2, 3, 3, 0 },
            //Back face
            {4, 5, 5, 6, 6, 7, 7, 4 },
            //Top face
            {3, 2, 2, 6, 6, 7, 7, 3 },
            //Bottom face
            {0, 1, 1, 5, 5, 4, 4, 0 },
            //Right face
            {1, 5, 5, 6, 6, 2, 2, 1 },
            //Left face
            {0, 4, 4, 7, 7, 3, 3, 0 }
        };

        private int[,] indicies2 =
{
            //Front face
            {0, 1, 2, 3},
            //Back face
            {4, 5, 6, 7},
            //Top face
            {3, 2, 6, 7},
            //Bottom face
            {0, 1, 5, 4},
            //Right face
            {1, 5, 6, 2},
            //Left face
            {0, 4, 7, 3}
        };

        private Matrix coeffs(Matrix matrix)
        {
            Matrix matrixInv = Matrix.inverse3x3(matrix);
            Matrix d = new Matrix(new float[3, 1] { { -1 }, { -1 }, { -1 } });
            return matrixInv * d;
        }

        private Matrix V;

        private float side;
        public float Side
        {
            get { return side; }
            set { side = value; }
        }
        private Matrix[] vertecies;
        private Matrix centerPoint;
        private SolidBrush[] brushes =
        {
            new SolidBrush(Color.Red),
            new SolidBrush(Color.Orange),
            new SolidBrush(Color.Yellow),
            new SolidBrush(Color.Green),
            new SolidBrush(Color.Blue),
            new SolidBrush(Color.Purple)
        };
    }
}

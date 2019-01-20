using System;
using System.Collections.Generic;
using System.Text;

namespace V1
{
    partial class LifeForm
    {
        // aOld Matrix mit der n-ten Generation
        // aNew Matrix mit der n+1-ten Generation
        void CalcNextGeneration(bool[,] aOld, bool[,] aNew)
        {
            _CC = aOld; // sicherstellen, daﬂ _CC=aOld ist

            for (int i = 0; i < MAX_CELLS; i++)
            {
                for (int j = 0; j < MAX_CELLS; j++)
                {
                    //Eine tote Zelle mit genau drei lebenden Nachbarn wird in der Folgegeneration neu geboren.
                    if (GetNeighbourCount(i, j) == 3 && aOld[i, j] == false)
                        aNew[i, j] = true;

                    //Eine lebende Zelle mit zwei oder drei lebenden Nachbarn bleibt in der Folgegeneration am Leben.
                    if (GetNeighbourCount(i, j) == 3 && aOld[i, j] == true || GetNeighbourCount(i, j) == 2 && aOld[i, j] == true)
                        aNew[i, j] = true;

                    //Lebende Zellen mit mehr als drei lebenden Nachbarn sterben in der Folgegeneration an ‹berbevˆlkerung.
                    else if (GetNeighbourCount(i, j) > 3 && aOld[i, j] == true)
                        aNew[i, j] = false;

                    //Lebende Zellen mit weniger als zwei lebenden Nachbarn sterben in der Folgegeneration an Einsamkeit.
                    else if (GetNeighbourCount(i, j) < 2 && aOld[i, j] == true)
                        aNew[i, j] = false;

                }
            }

        }


        void ClearCells(bool[,] aCells)
        {
            // alle Zellen von aCells auf false
            for (int i = 0; i < MAX_CELLS; i++)
            {
                for (int j = 0; j < MAX_CELLS; j++)
                {
                    aCells[i, j] = false;
                }
            }



        }

        // cells of _CC
        void TurnCellOnOff(int aX, int aY)
        {
            // Cell an der Stelle aX,aY toggeln ( Umschalten )
            if (aX < MAX_CELLS && aY < MAX_CELLS)
            {
                _CC[aX, aY] = !_CC[aX, aY];
                //DEBUG
                _Neighboar.Text = GetNeighbourCount(aX, aY).ToString();
            }




        }

        // cells of _CC
        int GetNeighbourCount(int i, int j)
        {
            // 
            int summe = 0;


            if (ValOf(i - 1, j - 1))
                summe++;

            if (ValOf(i - 1, j))
                summe++;

            if (ValOf(i, j - 1))
                summe++;

            if (ValOf(i + 1, j + 1))
                summe++;

            if (ValOf(i + 1, j))
                summe++;

            if (ValOf(i, j + 1))
                summe++;

            if (ValOf(i - 1, j + 1))
                summe++;

            if (ValOf(i + 1, j - 1))
                summe++;


            // wieviele lebende Nachbarn hat Cell(i,j)
            return summe;
        }

        // Ist Cell(i,j) von _CC on oder off ?
        // mit richtiger Behandlung von i,j<0 und i,j>=MAX_CELLS
        bool ValOf(int i, int j)
        {
            if (i < 0)
                i = MAX_CELLS - 1;

            if (j < 0)
                j = MAX_CELLS - 1;

            if (i >= MAX_CELLS)
                i = 0;

            if (j >= MAX_CELLS)
                j = 0;


            //Wenn der Rand erreicht wird false

            return _CC[i, j];
        }

    }
}

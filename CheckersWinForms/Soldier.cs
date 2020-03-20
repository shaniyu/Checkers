using System;
using System.Collections.Generic;

namespace CheckersWinForms
{
    public class Soldier
    {
        private char m_SoldierSign;
        private bool m_IsKing = false;
        private ePlayer m_BelongToPlayer;
        private List<eMove> m_PossibleMoves = new List<eMove>(4);
        private int[] m_PositionOnBoard = new int[2];

        public Soldier(char i_SoldierSign, params int[] i_PositionOnBoard)
        {
            m_SoldierSign = i_SoldierSign;
            if (i_SoldierSign == (char)ePlayerSigns.Player1Soldier)
            {
                m_BelongToPlayer = ePlayer.Player1;
                m_PossibleMoves.Add(eMove.UpLeft);
                m_PossibleMoves.Add(eMove.UpRight);
            }
            else
            {
                m_BelongToPlayer = ePlayer.Player2;
                m_PossibleMoves.Add(eMove.DownLeft);
                m_PossibleMoves.Add(eMove.DownRight);
            }

            m_PositionOnBoard[0] = i_PositionOnBoard[0];
            m_PositionOnBoard[1] = i_PositionOnBoard[1];
        }

        public char SoldierSign
        {
            get
            {
                return m_SoldierSign;
            }

            set
            {
                m_SoldierSign = value;
            }
        }

        public bool IsKing
        {
            get
            {
                return m_IsKing;
            }

            set
            {
                m_IsKing = value;
                if (value)
                {
                    updatePossibleMoves();
                    updateSoldierSignToKing();
                }
            }
        }

        private void updatePossibleMoves()
        {
            if (m_BelongToPlayer == ePlayer.Player1)
            {
                m_PossibleMoves.Add(eMove.DownLeft);
                m_PossibleMoves.Add(eMove.DownRight);
            }
            else
            {
                m_PossibleMoves.Add(eMove.UpLeft);
                m_PossibleMoves.Add(eMove.UpRight);
            }
        }

        private void updateSoldierSignToKing()
        {
            if (m_BelongToPlayer == ePlayer.Player1)
            {
                m_SoldierSign = (char)ePlayerSigns.Player1King;
            }
            else
            {
                m_SoldierSign = (char)ePlayerSigns.Player2King;
            }
        }

        public char GetSoldierSign()
        {
            return SoldierSign;
        }

        public ePlayer GetWhichPlayerSoldierBelongs()
        {
            return m_BelongToPlayer;
        }

        public List<eMove> GetPossibleMoves()
        {
            return m_PossibleMoves;
        }

        public void SetPositionOnBoard(params int[] i_PositionOnBoard)
        {
            m_PositionOnBoard[0] = i_PositionOnBoard[0];
            m_PositionOnBoard[1] = i_PositionOnBoard[1];
        }

        public int[] GetPositionOnBoard()
        {
            return m_PositionOnBoard;
        }
    }
}

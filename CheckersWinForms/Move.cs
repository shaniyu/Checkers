using System;

namespace CheckersWinForms
{
    public class Move
    {
        private int m_RowToMoveFrom;
        private int m_RowToMoveTo;
        private int m_ColToMoveFrom;
        private int m_ColToMoveTo;

        public Move(int i_RowToMoveTo, int i_RowToMoveFrom, int i_ColToMoveTo, int i_ColToMoveFrom)
        {
            m_RowToMoveFrom = i_RowToMoveFrom;
            m_RowToMoveTo = i_RowToMoveTo;
            m_ColToMoveFrom = i_ColToMoveFrom;
            m_ColToMoveTo = i_ColToMoveTo;
        }

        public int RowToMoveFrom
        {
            get
            {
                return m_RowToMoveFrom;
            }

            set
            {
                m_RowToMoveFrom = value;
            }
        }

        public int RowToMoveTo
        {
            get
            {
                return m_RowToMoveTo;
            }

            set
            {
                m_RowToMoveTo = value;
            }
        }

        public int ColToMoveFrom
        {
            get
            {
                return m_ColToMoveFrom;
            }

            set
            {
                m_ColToMoveFrom = value;
            }
        }

        public int ColToMoveTo
        {
            get
            {
                return m_ColToMoveTo;
            }

            set
            {
                m_ColToMoveTo = value;
            }
        }
    }
}

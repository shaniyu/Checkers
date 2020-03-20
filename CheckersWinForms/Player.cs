using System;

namespace CheckersWinForms
{
     public class Player
     {
          private bool m_IsComputer = false;
          private string m_Name;
          private int m_Score = 0;
          private int m_CurrentNumOfSoldiers;
          private int m_CurrentNumOfKings;
          private ePlayer m_WhichPlayerAmI;
          private char m_PlayersSoldierSign;
          private char m_PlayersKingSign;
          private bool m_IsCurrentlyWinning;

          public char PlayersSoldierSign
          {
               get
               {
                    return m_PlayersSoldierSign;
               }

               set
               {
                    m_PlayersSoldierSign = value;
               }
          }

          public char PlayersKingSign
          {
               get
               {
                    return m_PlayersKingSign;
               }

               set
               {
                    m_PlayersKingSign = value;
               }
          }

          public Player(ePlayer i_WhichPlayer)
          {
               m_WhichPlayerAmI = i_WhichPlayer;

               if (m_WhichPlayerAmI == ePlayer.Player1)
               {
                    m_PlayersSoldierSign = (char)ePlayerSigns.Player1Soldier;
                    m_PlayersKingSign = (char)ePlayerSigns.Player1King;
               }
               else
               {
                    m_PlayersSoldierSign = (char)ePlayerSigns.Player2Soldier;
                    m_PlayersKingSign = (char)ePlayerSigns.Player2King;
               }
          }

          public ePlayer WhichPlayerAmI
          {
               get
               {
                    return m_WhichPlayerAmI;
               }

               set
               {
                    m_WhichPlayerAmI = value;
               }
          }
         
          public string PlayerName
          {
               get
               {
                    return m_Name;
               }

               set
               {
                    m_Name = value;
               }
          }

          public bool IsComputer
          {
               get
               {
                    return m_IsComputer;
               }

               set
               {
                    m_IsComputer = value;
               }
          }

          public int NumberOfSoldiers
          {
               get
               {
                    return m_CurrentNumOfSoldiers;
               }

               set
               {
                    m_CurrentNumOfSoldiers = value;
               }
          }

          public int NumberOfKings
          {
               get
               {
                    return m_CurrentNumOfKings;
               }

               set
               {
                    m_CurrentNumOfKings = value;
               }
          }

          public int Score
          {
               get
               {
                    return m_Score;
               }

               set
               {
                    m_Score = value;
               }
          }

          public bool IsCurrentlyWinning
          {
               get
               {
                    return m_IsCurrentlyWinning;
               }

               set
               {
                    m_IsCurrentlyWinning = value;
               }
          }
     }
}

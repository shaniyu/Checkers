using System;
using System.Collections.Generic;

namespace CheckersWinForms
{
     public class GameLogic
     {
          private const int k_ScoreForKing = 4;
          private Soldier[,] m_GameBoard;

          private void initializeGameBoard(int i_BoardSize)
          {
               m_GameBoard = new Soldier[i_BoardSize, i_BoardSize];
               setGameBoard();
          }

          private void setGameBoard()
          {
               int rowsForO;
               int rows;

               ////For loop that will run the number of rows that need to be filled with 'O' soldiers
               for (rowsForO = 0; rowsForO < (m_GameBoard.GetLength(1) / 2) - 1; rowsForO++)
               {
                    for (int cols = 0; cols < m_GameBoard.GetLength(0); cols++)
                    {
                         fillWithSpaceOrSoldier(rowsForO, cols, (char)ePlayerSigns.Player2Soldier);
                    }
               }
               ////Fill 2 middle rows with spaces only
               for (rows = rowsForO; rows < rowsForO + 2; rows++)
               {
                    for (int cols = 0; cols < m_GameBoard.GetLength(0); cols++)
                    {
                         m_GameBoard[rows, cols] = null;
                    }
               }
               ////For loop that will run the number of rows that need to be filled with 'X' soldiers
               for (int rowsForX = rows; rowsForX < m_GameBoard.GetLength(1); rowsForX++)
               {
                    for (int cols = 0; cols < m_GameBoard.GetLength(0); cols++)
                    {
                         fillWithSpaceOrSoldier(rowsForX, cols, (char)ePlayerSigns.Player1Soldier);
                    }
               }
          }

          private void fillWithSpaceOrSoldier(int i_Rows, int i_Cols, char i_SoldierSign)
          {
               if (i_Rows % 2 == 0)
               {
                    if (i_Cols % 2 != 0)
                    {
                         m_GameBoard[i_Rows, i_Cols] = new Soldier(i_SoldierSign, i_Rows, i_Cols);
                    }
                    else
                    {
                         m_GameBoard[i_Rows, i_Cols] = null;
                    }
               }
               else
               {
                    if (i_Cols % 2 != 0)
                    {
                         m_GameBoard[i_Rows, i_Cols] = null;
                    }
                    else
                    {
                         m_GameBoard[i_Rows, i_Cols] = new Soldier(i_SoldierSign, i_Rows, i_Cols);
                    }
               }
          }

          public bool PerformTheChosenMoveAndCheckIfCanPerformAnotherMoveInCurrentTurn(Move i_MoveToPerform, Player i_CurrentTurn, Player i_SecondPlayer, out Move o_NextValidMoveInCurrentTurn)
          {
               bool wasASoldierCaptured = false;
               bool isCapturedSoldierAKing;
               bool canPerformAnotherMoveInCurrentTurn = false;

               o_NextValidMoveInCurrentTurn = new Move(0, 0, 0, 0);
               checkIfSoldierShouldBeKingAfterMoveAndUpdate(i_MoveToPerform, i_CurrentTurn);
               if (i_MoveToPerform.RowToMoveTo - i_MoveToPerform.RowToMoveFrom == 2 || i_MoveToPerform.RowToMoveTo - i_MoveToPerform.RowToMoveFrom == -2)
               {
                    isCapturedSoldierAKing = m_GameBoard[i_MoveToPerform.RowToMoveFrom + ((i_MoveToPerform.RowToMoveTo - i_MoveToPerform.RowToMoveFrom) / 2), i_MoveToPerform.ColToMoveFrom + ((i_MoveToPerform.ColToMoveTo - i_MoveToPerform.ColToMoveFrom) / 2)].IsKing;
                    decreaseNumberOfKingsOrSoldiers(isCapturedSoldierAKing, i_SecondPlayer);
                    m_GameBoard[i_MoveToPerform.RowToMoveFrom + ((i_MoveToPerform.RowToMoveTo - i_MoveToPerform.RowToMoveFrom) / 2), i_MoveToPerform.ColToMoveFrom + ((i_MoveToPerform.ColToMoveTo - i_MoveToPerform.ColToMoveFrom) / 2)] = null;
                    wasASoldierCaptured = true;
               }

               m_GameBoard[i_MoveToPerform.RowToMoveFrom, i_MoveToPerform.ColToMoveFrom].SetPositionOnBoard(i_MoveToPerform.RowToMoveTo, i_MoveToPerform.ColToMoveTo);
               m_GameBoard[i_MoveToPerform.RowToMoveTo, i_MoveToPerform.ColToMoveTo] = m_GameBoard[i_MoveToPerform.RowToMoveFrom, i_MoveToPerform.ColToMoveFrom];
               m_GameBoard[i_MoveToPerform.RowToMoveFrom, i_MoveToPerform.ColToMoveFrom] = null;
               if (wasASoldierCaptured)
               {
                    canPerformAnotherMoveInCurrentTurn = checkIfCurrentSoldierCanCaptureOpponentSoldier(GetGameBoard()[i_MoveToPerform.RowToMoveTo, i_MoveToPerform.ColToMoveTo], out o_NextValidMoveInCurrentTurn);
               }

               calculateCurrentScoresAndUpdateWhoIsCurrentlyWinning(i_CurrentTurn, i_SecondPlayer);
               return canPerformAnotherMoveInCurrentTurn;
          }

          private bool checkIfSoldierShouldBeKingAfterMoveAndUpdate(Move i_MoveToCheck, Player i_CurrentTurn)
          {
               bool hasSoldierBecomeKing = false;
               if (!m_GameBoard[i_MoveToCheck.RowToMoveFrom, i_MoveToCheck.ColToMoveFrom].IsKing)
               {
                    hasSoldierBecomeKing = i_MoveToCheck.RowToMoveTo == 0 || i_MoveToCheck.RowToMoveTo == m_GameBoard.GetLength(0) - 1;
               }

               if (hasSoldierBecomeKing)
               {
                    m_GameBoard[i_MoveToCheck.RowToMoveFrom, i_MoveToCheck.ColToMoveFrom].IsKing = true;
                    increaseNumberOfKings(i_CurrentTurn);
               }

               return hasSoldierBecomeKing;
          }

          private void increaseNumberOfKings(Player i_CurrentTurn)
          {
               i_CurrentTurn.NumberOfKings++;
               i_CurrentTurn.NumberOfSoldiers--;
          }

          private void decreaseNumberOfKingsOrSoldiers(bool i_IsCapturedSoldierAKing, Player i_PlayerToUpdate)
          {
               if (i_IsCapturedSoldierAKing)
               {
                    decreaseNumberOfKings(i_PlayerToUpdate);
               }
               else
               {
                    decreaseNumberOfSoldiers(i_PlayerToUpdate);
               }
          }

          private void decreaseNumberOfKings(Player i_PlayerToUpate)
          {
               i_PlayerToUpate.NumberOfKings--;
          }

          private void decreaseNumberOfSoldiers(Player i_PlayerToUpdate)
          {
               i_PlayerToUpdate.NumberOfSoldiers--;
          }

          public Soldier[,] GetGameBoard()
          {
               return m_GameBoard;
          }

          private bool checkIfCurrentSoldierCanCaptureOpponentSoldier(Soldier i_CurrentSoldier, out Move o_MoveThatWillResultInCapture)
          {
               bool canCurrentSoldierCaptureOpponentSoldier = false;
               int rowInBoard = i_CurrentSoldier.GetPositionOnBoard()[0];
               int colInBoard = i_CurrentSoldier.GetPositionOnBoard()[1];

               o_MoveThatWillResultInCapture = new Move(0, i_CurrentSoldier.GetPositionOnBoard()[0], 0, i_CurrentSoldier.GetPositionOnBoard()[1]);
               foreach (eMove possibleMoves in i_CurrentSoldier.GetPossibleMoves())
               {
                    if (!canCurrentSoldierCaptureOpponentSoldier)
                    {
                         switch (possibleMoves)
                         {
                              case eMove.UpLeft:
                                   canCurrentSoldierCaptureOpponentSoldier = checkIfOpponentSoldierIsInPossibleCapturePositionAccordingToSoldiersPlayer(i_CurrentSoldier, -1, -1);
                                   o_MoveThatWillResultInCapture.RowToMoveTo = rowInBoard - 2;
                                   o_MoveThatWillResultInCapture.ColToMoveTo = colInBoard - 2;
                                   break;
                              case eMove.UpRight:
                                   canCurrentSoldierCaptureOpponentSoldier = checkIfOpponentSoldierIsInPossibleCapturePositionAccordingToSoldiersPlayer(i_CurrentSoldier, -1, 1);
                                   o_MoveThatWillResultInCapture.RowToMoveTo = rowInBoard - 2;
                                   o_MoveThatWillResultInCapture.ColToMoveTo = colInBoard + 2;
                                   break;
                              case eMove.DownLeft:
                                   canCurrentSoldierCaptureOpponentSoldier = checkIfOpponentSoldierIsInPossibleCapturePositionAccordingToSoldiersPlayer(i_CurrentSoldier, 1, -1);
                                   o_MoveThatWillResultInCapture.RowToMoveTo = rowInBoard + 2;
                                   o_MoveThatWillResultInCapture.ColToMoveTo = colInBoard - 2;
                                   break;
                              case eMove.DownRight:
                                   canCurrentSoldierCaptureOpponentSoldier = checkIfOpponentSoldierIsInPossibleCapturePositionAccordingToSoldiersPlayer(i_CurrentSoldier, 1, 1);
                                   o_MoveThatWillResultInCapture.RowToMoveTo = rowInBoard + 2;
                                   o_MoveThatWillResultInCapture.ColToMoveTo = colInBoard + 2;
                                   break;
                              default:
                                   break;
                         }
                    }
               }

               return canCurrentSoldierCaptureOpponentSoldier;
          }

          private bool checkIfOpponentSoldierIsInPossibleCapturePositionAccordingToSoldiersPlayer(Soldier i_CurrentSoldier, int i_RowOffset, int i_ColOffset)
          {
               bool isOpponentSoldierIsInPossibleCapturePositionAccordingToSoldiersPlayer;
               int rowInBoard = i_CurrentSoldier.GetPositionOnBoard()[0];
               int colInBoard = i_CurrentSoldier.GetPositionOnBoard()[1];

               if (i_CurrentSoldier.GetWhichPlayerSoldierBelongs() == ePlayer.Player1)
               {
                    isOpponentSoldierIsInPossibleCapturePositionAccordingToSoldiersPlayer = checkIfOpponentSoldierIsInPossibleCapturePosition(rowInBoard, colInBoard, i_RowOffset, i_ColOffset, (char)ePlayerSigns.Player2Soldier, (char)ePlayerSigns.Player2King);
               }
               else
               {
                    isOpponentSoldierIsInPossibleCapturePositionAccordingToSoldiersPlayer = checkIfOpponentSoldierIsInPossibleCapturePosition(rowInBoard, colInBoard, i_RowOffset, i_ColOffset, (char)ePlayerSigns.Player1Soldier, (char)ePlayerSigns.Player1King);
               }

               return isOpponentSoldierIsInPossibleCapturePositionAccordingToSoldiersPlayer;
          }

          private bool checkIfOpponentSoldierIsInPossibleCapturePosition(int i_CurrRow, int i_CurrCol, int i_RowOffset, int i_ColOffset, char i_FirstPossibleCharOfOpponentPlayer, char i_SecondPossibleCharOfOpponentPlayer)
          {
               bool IsOpponentSoldierIsInPossibleCapturePosition = false;

               if (i_CurrRow + i_RowOffset < m_GameBoard.GetLength(0) && i_CurrCol + i_ColOffset < m_GameBoard.GetLength(1)
                   && i_CurrRow + i_RowOffset >= 0 && i_CurrCol + i_ColOffset >= 0)
               {
                    if (m_GameBoard[i_CurrRow + i_RowOffset, i_CurrCol + i_ColOffset] != null)
                    {
                         if (m_GameBoard[i_CurrRow + i_RowOffset, i_CurrCol + i_ColOffset].GetSoldierSign() == i_FirstPossibleCharOfOpponentPlayer || m_GameBoard[i_CurrRow + i_RowOffset, i_CurrCol + i_ColOffset].GetSoldierSign() == i_SecondPossibleCharOfOpponentPlayer)
                         {
                              if (i_CurrRow + i_RowOffset + i_RowOffset < m_GameBoard.GetLength(0) && i_CurrCol + i_ColOffset + i_ColOffset < m_GameBoard.GetLength(1)
                                  && i_CurrRow + i_RowOffset + i_RowOffset >= 0 && i_CurrCol + i_ColOffset + i_ColOffset >= 0)
                              {
                                   if (m_GameBoard[i_CurrRow + i_RowOffset + i_RowOffset, i_CurrCol + i_ColOffset + i_ColOffset] == null)
                                   {
                                        IsOpponentSoldierIsInPossibleCapturePosition = true;
                                   }
                              }
                         }
                    }
               }

               return IsOpponentSoldierIsInPossibleCapturePosition;
          }

          private Move randMoveForComputer(Player i_CurrentTurn)
          {
               Move nextMove = new Move(0, 0, 0, 0);
               List<Move> validMoves;

               validMoves = getListOfValidMovesForAPlayer(i_CurrentTurn);
               if (validMoves.Count > 0)
               {
                    Random randInt = new Random();
                    int indexOfChosenMove = randInt.Next(validMoves.Count);
                    nextMove = validMoves[indexOfChosenMove];
               }

               return nextMove;
          }

          public List<Move> PlayAsComputer(Player i_CurrentTurn, Player i_SecondPlayer)
          {
               List<Move> computersMove = new List<Move>(0);
               Move computersNextValidMoveInCaseOfMultipleCaptures;
               bool canPerformAnotherMoveInCurrentTurn = true;
               int indexOfLastMove = 0;

               computersMove.Add(randMoveForComputer(i_CurrentTurn));
               while (canPerformAnotherMoveInCurrentTurn)
               {
                    canPerformAnotherMoveInCurrentTurn = PerformTheChosenMoveAndCheckIfCanPerformAnotherMoveInCurrentTurn(computersMove[indexOfLastMove], i_CurrentTurn, i_SecondPlayer, out computersNextValidMoveInCaseOfMultipleCaptures);
                    indexOfLastMove++;
                    if (canPerformAnotherMoveInCurrentTurn)
                    {
                         computersMove.Add(computersNextValidMoveInCaseOfMultipleCaptures);
                         canPerformAnotherMoveInCurrentTurn = PerformTheChosenMoveAndCheckIfCanPerformAnotherMoveInCurrentTurn(computersMove[indexOfLastMove], i_CurrentTurn, i_SecondPlayer, out computersNextValidMoveInCaseOfMultipleCaptures);
                         indexOfLastMove++;
                    }
               }

               return computersMove;
          }

          private List<Move> getListOfValidMovesForAPlayer(Player i_CurrentTurn)
          {
               bool isThereAValidMove = false;
               List<Move> validMoves = new List<Move>(1);
               Move moveThatMightResultInCapture;

               isThereAValidMove = checkIfCurrentPlayerCanCaptureOpponentsSoldier(i_CurrentTurn, out moveThatMightResultInCapture);
               if (isThereAValidMove)
               {
                    validMoves.Add(moveThatMightResultInCapture);
               }
               else
               {
                    for (int i = 0; i < m_GameBoard.GetLength(0); i++)
                    {
                         for (int j = 0; j < m_GameBoard.GetLength(1); j++)
                         {
                              if (m_GameBoard[i, j] != null &&
                                  (m_GameBoard[i, j].GetSoldierSign() == i_CurrentTurn.PlayersKingSign ||
                                  m_GameBoard[i, j].GetSoldierSign() == i_CurrentTurn.PlayersSoldierSign))
                              {
                                   foreach (eMove move in m_GameBoard[i, j].GetPossibleMoves())
                                   {
                                        ////tempErrorNumber is a temporary variable used in order to call to CheckIfMoveIsValid
                                        ////we don't care about the error in the current function, we just want to check if
                                        ////there is a valid move
                                        int tempErrorNumber;
                                        int optionalRowToMoveTo, optionalColToMoveTo;
                                        findNewPositionAccordingToEMove(out optionalRowToMoveTo, out optionalColToMoveTo, move, m_GameBoard[i, j]);
                                        if (optionalColToMoveTo >= 0 && optionalColToMoveTo < m_GameBoard.GetLength(1) &&
                                            optionalRowToMoveTo >= 0 && optionalRowToMoveTo < m_GameBoard.GetLength(0))
                                        {
                                             Move optionalMove = new Move(optionalRowToMoveTo, m_GameBoard[i, j].GetPositionOnBoard()[0], optionalColToMoveTo, m_GameBoard[i, j].GetPositionOnBoard()[1]);
                                             isThereAValidMove = CheckIfMoveIsValid(optionalMove, i_CurrentTurn, out tempErrorNumber);
                                             if (isThereAValidMove)
                                             {
                                                  validMoves.Add(optionalMove);
                                             }
                                        }
                                   }
                              }
                         }
                    }
               }

               return validMoves;
          }

          private void findNewPositionAccordingToEMove(out int o_RowToMoveTo, out int o_ColToMoveTo, eMove i_CurrentEMove, Soldier i_CurrentSoldier)
          {
               switch (i_CurrentEMove)
               {
                    case eMove.UpLeft:
                         o_RowToMoveTo = i_CurrentSoldier.GetPositionOnBoard()[0] - 1;
                         o_ColToMoveTo = i_CurrentSoldier.GetPositionOnBoard()[1] - 1;
                         break;
                    case eMove.UpRight:
                         o_RowToMoveTo = i_CurrentSoldier.GetPositionOnBoard()[0] - 1;
                         o_ColToMoveTo = i_CurrentSoldier.GetPositionOnBoard()[1] + 1;
                         break;
                    case eMove.DownLeft:
                         o_RowToMoveTo = i_CurrentSoldier.GetPositionOnBoard()[0] + 1;
                         o_ColToMoveTo = i_CurrentSoldier.GetPositionOnBoard()[1] - 1;
                         break;
                    case eMove.DownRight:
                         o_RowToMoveTo = i_CurrentSoldier.GetPositionOnBoard()[0] + 1;
                         o_ColToMoveTo = i_CurrentSoldier.GetPositionOnBoard()[1] + 1;
                         break;
                    default:
                         o_RowToMoveTo = i_CurrentSoldier.GetPositionOnBoard()[0];
                         o_ColToMoveTo = i_CurrentSoldier.GetPositionOnBoard()[1];
                         break;
               }
          }

          private void initialNumberOfSoldiers(int i_BoardSize, Player i_Player1, Player i_Player2)
          {
               int numberOfSoliersInRow = i_BoardSize / 2;
               int numberOfSoldiers = ((i_BoardSize / 2) - 1) * numberOfSoliersInRow;

               i_Player1.NumberOfSoldiers = numberOfSoldiers;
               i_Player1.NumberOfKings = 0;
               i_Player2.NumberOfSoldiers = numberOfSoldiers;
               i_Player2.NumberOfKings = 0;
          }

          public bool CheckIfMoveIsValid(Move i_MoveToCheck, Player i_CurrentPlayer, out int o_ErrorNumber)
          {
               bool isMoveValid = true;
               o_ErrorNumber = (int)eErrors.InvalidMovingFromPosition;

               isMoveValid = checkIfSoldierInMovingFromPositionBelongsToCurrentPlayer(i_MoveToCheck, i_CurrentPlayer);
               if (!isMoveValid)
               {
                    o_ErrorNumber = (int)eErrors.InvalidMovingFromPosition;
               }
               else
               {
                    isMoveValid = checkIfMovingToPositionIsFree(i_MoveToCheck);
                    if (!isMoveValid)
                    {
                         o_ErrorNumber = (int)eErrors.MovingToPositionIsNotFree;
                    }
                    else
                    {
                         bool didSoldierCaptureOpponentSoldier;
                         isMoveValid = checkIfDistanceBetweenMovingFromAndMovingToPositionsIsValid(i_MoveToCheck, out didSoldierCaptureOpponentSoldier);
                         if (!isMoveValid)
                         {
                              o_ErrorNumber = (int)eErrors.InvalidMovingToPosition;
                         }
                         else
                         {
                              if (!didSoldierCaptureOpponentSoldier)
                              {
                                   ////The following temp variable are used in order to call to CheckIfCurrentPlayerCanCaptureOpponentsSoldier
                                   ////and aren't used later (we don't care about the move that should be done in order to
                                   ////capture another soldier because it should be entered by the computer
                                   Move tempMove;
                                   isMoveValid = !checkIfCurrentPlayerCanCaptureOpponentsSoldier(i_CurrentPlayer, out tempMove);
                                   if (!isMoveValid)
                                   {
                                        o_ErrorNumber = (int)eErrors.PlayerCanCaptureOpponentSoldier;
                                   }
                              }
                         }
                    }
               }

               return isMoveValid;
          }

          private bool checkIfSoldierInMovingFromPositionBelongsToCurrentPlayer(Move i_MoveToCheck, Player i_CurrentPlayer)
          {
               bool isSoldierInMovingFromPositionBelongsToCurrentPlayer = true;

               isSoldierInMovingFromPositionBelongsToCurrentPlayer =
                   m_GameBoard[i_MoveToCheck.RowToMoveFrom, i_MoveToCheck.ColToMoveFrom] != null &&
                   (m_GameBoard[i_MoveToCheck.RowToMoveFrom, i_MoveToCheck.ColToMoveFrom].GetSoldierSign() == i_CurrentPlayer.PlayersSoldierSign
                   || m_GameBoard[i_MoveToCheck.RowToMoveFrom, i_MoveToCheck.ColToMoveFrom].GetSoldierSign() == i_CurrentPlayer.PlayersKingSign);

               return isSoldierInMovingFromPositionBelongsToCurrentPlayer;
          }

          private bool checkIfMovingToPositionIsFree(Move i_MoveToCheck)
          {
               bool isMovingToPositionFree = true;

               isMovingToPositionFree = m_GameBoard[i_MoveToCheck.RowToMoveTo, i_MoveToCheck.ColToMoveTo] == null;

               return isMovingToPositionFree;
          }

          private bool checkIfDistanceBetweenMovingFromAndMovingToPositionsIsValid(Move i_MoveToCheck, out bool o_DidSoldierCaptureOpponentSoldier)
          {
               bool isChosenMoveAPossibleMoveOfCurrentSoldier;
               bool canCurrentSoldierCaptureAnOpponentSoldier;
               eMove convertedDirection;
               int rowAddition = i_MoveToCheck.RowToMoveTo - i_MoveToCheck.RowToMoveFrom;
               int colAddition = i_MoveToCheck.ColToMoveTo - i_MoveToCheck.ColToMoveFrom;

               o_DidSoldierCaptureOpponentSoldier = false;
               convertedDirection = convertDirectionToEMove(rowAddition, colAddition);
               isChosenMoveAPossibleMoveOfCurrentSoldier = checkIfCurrentMoveIsAPossibleMoveOfTheCurrentSoldier(m_GameBoard[i_MoveToCheck.RowToMoveFrom, i_MoveToCheck.ColToMoveFrom], convertedDirection);
               if (!isChosenMoveAPossibleMoveOfCurrentSoldier)
               {
                    ////tempMove is temporary variable used in order to call to CheckIfCurrentSoldierCanCaptureOpponentSoldier
                    ////We already have the rowToMoveTo and the colToMoveTo that were chosen by the player so we don't need
                    ////the ones that will complete the move that should be done in order to capture an opponent soldier
                    Move tempMove;
                    canCurrentSoldierCaptureAnOpponentSoldier = checkIfCurrentSoldierCanCaptureOpponentSoldier(m_GameBoard[i_MoveToCheck.RowToMoveFrom, i_MoveToCheck.ColToMoveFrom], out tempMove);
                    if (canCurrentSoldierCaptureAnOpponentSoldier)
                    {
                         if (rowAddition % 2 == 0 && colAddition % 2 == 0)
                         {
                              convertedDirection = convertDirectionToEMove(rowAddition / 2, colAddition / 2);
                              isChosenMoveAPossibleMoveOfCurrentSoldier = checkIfCurrentMoveIsAPossibleMoveOfTheCurrentSoldier(m_GameBoard[i_MoveToCheck.RowToMoveFrom, i_MoveToCheck.ColToMoveFrom], convertedDirection);
                              if (isChosenMoveAPossibleMoveOfCurrentSoldier)
                              {
                                   char capturedSoldierSign = m_GameBoard[i_MoveToCheck.RowToMoveFrom + (rowAddition / 2), i_MoveToCheck.ColToMoveFrom + (colAddition / 2)].GetSoldierSign();
                                   char currentSoldierSign = m_GameBoard[i_MoveToCheck.RowToMoveFrom, i_MoveToCheck.ColToMoveFrom].GetSoldierSign();
                                   if (((currentSoldierSign == (char)ePlayerSigns.Player1Soldier || currentSoldierSign == (char)ePlayerSigns.Player1King)
                                       && (capturedSoldierSign == (char)ePlayerSigns.Player2Soldier || capturedSoldierSign == (char)ePlayerSigns.Player2King))
                                       || ((currentSoldierSign == (char)ePlayerSigns.Player2Soldier || currentSoldierSign == (char)ePlayerSigns.Player2King)
                                       && (capturedSoldierSign == (char)ePlayerSigns.Player1Soldier || capturedSoldierSign == (char)ePlayerSigns.Player1King)))
                                   {
                                        o_DidSoldierCaptureOpponentSoldier = true;
                                        isChosenMoveAPossibleMoveOfCurrentSoldier = true;
                                   }
                              }
                         }
                    }
               }

               return isChosenMoveAPossibleMoveOfCurrentSoldier;
          }

          private eMove convertDirectionToEMove(int i_RowAddition, int i_ColAddition)
          {
               eMove convertedDirection;

               if (i_RowAddition == 1 && i_ColAddition == 1)
               {
                    convertedDirection = eMove.DownRight;
               }
               else if (i_RowAddition == 1 && i_ColAddition == -1)
               {
                    convertedDirection = eMove.DownLeft;
               }
               else if (i_RowAddition == -1 && i_ColAddition == 1)
               {
                    convertedDirection = eMove.UpRight;
               }
               else if (i_RowAddition == -1 && i_ColAddition == -1)
               {
                    convertedDirection = eMove.UpLeft;
               }
               else
               {
                    convertedDirection = eMove.Invalid;
               }

               return convertedDirection;
          }

          private bool checkIfCurrentMoveIsAPossibleMoveOfTheCurrentSoldier(Soldier i_CurrentSoldier, eMove i_CurrentMove)
          {
               bool isCurrentMoveIsAPossibleMoveOfTheCurrentSoldier = false;

               if (i_CurrentMove != eMove.Invalid)
               {
                    foreach (eMove move in i_CurrentSoldier.GetPossibleMoves())
                    {
                         if (!isCurrentMoveIsAPossibleMoveOfTheCurrentSoldier)
                         {
                              isCurrentMoveIsAPossibleMoveOfTheCurrentSoldier = i_CurrentMove == move;
                         }
                    }
               }

               return isCurrentMoveIsAPossibleMoveOfTheCurrentSoldier;
          }

          private bool checkIfCurrentPlayerCanCaptureOpponentsSoldier(Player i_CurrentPlayer, out Move o_MoveThatWillResultInCapture)
          {
               bool canCurrentPlayerCaptureOpponentSoldier = false;

               o_MoveThatWillResultInCapture = new Move(0, 0, 0, 0);

               for (int i = 0; i < m_GameBoard.GetLength(0) && !canCurrentPlayerCaptureOpponentSoldier; i++)
               {
                    for (int j = 0; j < m_GameBoard.GetLength(1) && !canCurrentPlayerCaptureOpponentSoldier; j++)
                    {
                         if (m_GameBoard[i, j] != null)
                         {
                              o_MoveThatWillResultInCapture.RowToMoveFrom = m_GameBoard[i, j].GetPositionOnBoard()[0];
                              o_MoveThatWillResultInCapture.ColToMoveFrom = m_GameBoard[i, j].GetPositionOnBoard()[1];
                              if (i_CurrentPlayer.PlayersSoldierSign == m_GameBoard[i, j].GetSoldierSign() ||
                                  i_CurrentPlayer.PlayersKingSign == m_GameBoard[i, j].GetSoldierSign())
                              {
                                   canCurrentPlayerCaptureOpponentSoldier = checkIfCurrentSoldierCanCaptureOpponentSoldier(m_GameBoard[i, j], out o_MoveThatWillResultInCapture);
                              }
                         }
                    }
               }

               return canCurrentPlayerCaptureOpponentSoldier;
          }

          private void calculateCurrentScoresAndUpdateWhoIsCurrentlyWinning(Player i_Player1, Player i_Player2)
          {
               int player1CurrentScore, player2CurrentScore;
               player1CurrentScore = (k_ScoreForKing * i_Player1.NumberOfKings) + i_Player1.NumberOfSoldiers;
               player2CurrentScore = (k_ScoreForKing * i_Player2.NumberOfKings) + i_Player2.NumberOfSoldiers;
               if (player1CurrentScore > player2CurrentScore)
               {
                    i_Player1.IsCurrentlyWinning = true;
                    i_Player2.IsCurrentlyWinning = false;
               }
               else
               {
                    i_Player1.IsCurrentlyWinning = false;
                    i_Player2.IsCurrentlyWinning = true;
               }
          }

          private void calculateAndUpdateWinnersScore(Player i_Player1, Player i_Player2)
          {
               int scoreAddition;

               if (i_Player1.IsCurrentlyWinning)
               {
                    scoreAddition = ((k_ScoreForKing * i_Player1.NumberOfKings) + i_Player1.NumberOfSoldiers) - ((k_ScoreForKing * i_Player2.NumberOfKings) + i_Player2.NumberOfSoldiers);
                    i_Player1.Score += scoreAddition;
               }
               else
               {
                    scoreAddition = ((k_ScoreForKing * i_Player2.NumberOfKings) + i_Player2.NumberOfSoldiers) - ((k_ScoreForKing * i_Player1.NumberOfKings) + i_Player1.NumberOfSoldiers);
                    i_Player2.Score += scoreAddition;
               }
          }

          private bool isThereAPlayerWithoutSoldiers(Player i_Player1, Player i_Player2)
          {
               bool isThereAPlayerWithoutSoldiers;

               isThereAPlayerWithoutSoldiers = (i_Player1.NumberOfKings == 0 && i_Player1.NumberOfSoldiers == 0) || (i_Player2.NumberOfKings == 0 && i_Player2.NumberOfSoldiers == 0);

               return isThereAPlayerWithoutSoldiers;
          }

          public bool CheckIfGameIsOverAndUpdateScore(Player i_Player1, Player i_Player2, bool i_DidPlayerQuit, out bool o_IsTie)
          {
               bool isGameOver;

               if (i_DidPlayerQuit)
               {
                    isGameOver = true;
                    o_IsTie = false;
               }
               else if (isThereAPlayerWithoutSoldiers(i_Player1, i_Player2))
               {
                    isGameOver = true;
                    o_IsTie = false;
               }
               else
               {
                    List<Move> validMoveForPlayer1;
                    List<Move> validMoveForPlayer2;

                    validMoveForPlayer1 = getListOfValidMovesForAPlayer(i_Player1);
                    validMoveForPlayer2 = getListOfValidMovesForAPlayer(i_Player2);
                    if (validMoveForPlayer1.Count == 0 && validMoveForPlayer2.Count == 0)
                    {
                         o_IsTie = true;
                         isGameOver = true;
                    }
                    else if (validMoveForPlayer1.Count == 0)
                    {
                         o_IsTie = false;
                         isGameOver = true;
                    }
                    else if (validMoveForPlayer2.Count == 0)
                    {
                         o_IsTie = false;
                         isGameOver = true;
                    }
                    else
                    {
                         o_IsTie = false;
                         isGameOver = false;
                    }
               }

               if (!o_IsTie && isGameOver)
               {
                    calculateAndUpdateWinnersScore(i_Player1, i_Player2);
               }

               return isGameOver;
          }

          public void StartNewGame(int i_BoardSize, Player i_Player1, Player i_Player2)
          {
               initialNumberOfSoldiers(i_BoardSize, i_Player1, i_Player2);
               initializeGameBoard(i_BoardSize);
               if (i_Player1.Score > i_Player2.Score)
               {
                    i_Player1.IsCurrentlyWinning = true;
                    i_Player2.IsCurrentlyWinning = false;
               }
               else
               {
                    i_Player2.IsCurrentlyWinning = true;
                    i_Player1.IsCurrentlyWinning = false;
               }
          }
     }
}

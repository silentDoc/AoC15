using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;
namespace AoC15.Day22
{

    enum Action
    { 
        MagicMissile =0, 
        Drain = 1, 
        Shield = 2, 
        Poison = 3, 
        Recharge = 4,
        EnemyAttack = 5
    }

    enum EndCondition
    {
        PlayerVictory = 0,
        EnemyVictory = 1,
        CannotCastAction = 2,
        RanOutOfMana = 3,
        SpentManaGreaterThanMin = 4,
        Unknown = 5,
        PlayerVictory_Poison = 6,
    }

    class Turn
    {
        static int Cost(Action action)
            => action switch
            {
                Action.MagicMissile => 53,
                Action.Drain => 73,
                Action.Shield => 113,
                Action.Poison => 173,
                Action.Recharge => 229,
                _ => 0
            };

        public bool PlayerTurn;
        public Action action;
        public EndCondition outcome = EndCondition.Unknown;

        public int PlayerHitPoints;
        public int PlayerCurrentManaPoints;
        public int PlayerSpentManaPoints;
        
        public int EnemyHitPoints;
        public int EnemyDamage;

        public bool GameEnded = false;
        public bool PlayerVictory = false;

        public int activeEffect_Shield = 0;
        public int activeEffect_Poison = 0;
        public int activeEffect_Recharge = 0;

        public List<Turn>? children = null;
        public Turn? parent = null;
        public int TurnCount = 0;

        public string OneLiner()
        {
            StringBuilder s = new();
            s.Append("T : " + TurnCount.ToString());
            s.Append(" - Action : " + action.ToString());
            s.Append(" - Player : " + PlayerHitPoints.ToString());
            s.Append(" vs Enemy : " + EnemyHitPoints.ToString());
            s.Append(" ; Cost : " + Cost(action).ToString());
            s.Append(" ; Av. Mana : " + PlayerCurrentManaPoints.ToString());
            s.Append(" ; Sp. Mana : " + PlayerSpentManaPoints.ToString());
            s.Append(" ;; " + outcome.ToString());
            return s.ToString();
        }

        public void AdvanceEffects(Turn previousTurn)
        {
            activeEffect_Shield = (previousTurn.activeEffect_Shield > 0) ? previousTurn.activeEffect_Shield - 1 : 0;
            activeEffect_Poison = (previousTurn.activeEffect_Poison > 0) ? previousTurn.activeEffect_Poison - 1 : 0;
            activeEffect_Recharge = (previousTurn.activeEffect_Recharge > 0) ? previousTurn.activeEffect_Recharge - 1 : 0;
        }

        public void AdvanceEffects()
        {
            if(activeEffect_Shield>0) activeEffect_Shield--;
            if(activeEffect_Poison>0) activeEffect_Poison--;
            if(activeEffect_Recharge>0) activeEffect_Recharge--;
        }

        public Turn Clone()
        {
            Turn retVal = new();
            retVal.PlayerTurn = PlayerTurn;
            retVal.action= action;
            retVal.outcome = outcome;
            
            retVal.PlayerHitPoints = PlayerHitPoints;
            retVal.PlayerCurrentManaPoints = PlayerCurrentManaPoints;
            retVal.PlayerSpentManaPoints = PlayerSpentManaPoints;
            
            retVal.EnemyHitPoints = EnemyHitPoints;
            retVal.EnemyDamage = EnemyDamage;
            
            retVal.GameEnded = GameEnded;
            retVal.PlayerVictory = PlayerVictory;

            retVal.activeEffect_Shield = activeEffect_Shield;
            retVal.activeEffect_Poison = activeEffect_Poison;
            retVal.activeEffect_Recharge = activeEffect_Recharge;

            retVal.TurnCount = TurnCount;

            return retVal;
        }
    }

    internal class RPGWizard
    {
        const int PoisonTurnDamage = 3;
        const int RechargeManaIncrease = 101;
        const int ShieldArmorIncrease = 7;
        int EnemyDamage = 0;
        int EnemyHitPoints = 0;

        int MinManaSpent = 999999;

        readonly List<Action> AllPlayerActions = new() { Action.MagicMissile, 
                                                      Action.Drain, 
                                                      Action.Poison, 
                                                      Action.Recharge, 
                                                      Action.Shield };

        List<List<Turn>> possibleGames = new();
        List<Turn> finalRounds = new();

        static int Cost(Action action)
            => action switch
            {
                Action.MagicMissile => 53,
                Action.Drain => 73,
                Action.Shield => 113,
                Action.Poison => 173,
                Action.Recharge => 229,
                _ => throw new Exception("Invalid action")
            };

        static int Damage(Action action)
           => action switch
           {
               Action.MagicMissile => 4,
               Action.Drain => 2,
               Action.Shield => 0,
               Action.Poison => 0,
               Action.Recharge => 0,
               _ => throw new Exception("Invalid action")
           };

        static int Timer(Action action)
           => action switch
           {
               Action.MagicMissile => 0,
               Action.Drain => 0,
               Action.Shield => 6,
               Action.Poison => 6,
               Action.Recharge => 5,
               _ => throw new Exception("Invalid action")
           };



        public Turn NextTurn(Turn previousTurn, Action currentAction)
        {
            Turn currentTurn = previousTurn.Clone();
            currentTurn.PlayerTurn = !previousTurn.PlayerTurn;
            currentTurn.action = currentAction;
            currentTurn.parent = previousTurn;
            currentTurn.TurnCount++;

            if (currentTurn.PlayerSpentManaPoints > MinManaSpent)
            {
                currentTurn.PlayerVictory = false;
                currentTurn.outcome = EndCondition.SpentManaGreaterThanMin;
                currentTurn.GameEnded = true;
                finalRounds.Add(currentTurn);
                return currentTurn;
            }

            bool shieldIsActive = false;

            // 1. Apply effects in place and advance timer
            if (currentTurn.activeEffect_Shield > 0)
                shieldIsActive = true;
            if (currentTurn.activeEffect_Recharge > 0)
                currentTurn.PlayerCurrentManaPoints += RechargeManaIncrease;
            if (currentTurn.activeEffect_Poison > 0)
                currentTurn.EnemyHitPoints -= PoisonTurnDamage;

            currentTurn.AdvanceEffects();

            // Check game over condition 1 - Poison kill
            if (currentTurn.EnemyHitPoints <= 0)
            {
                currentTurn.GameEnded = true;
                currentTurn.PlayerVictory = true;
                currentTurn.outcome = EndCondition.PlayerVictory_Poison;
                finalRounds.Add(currentTurn);

                if (currentTurn.PlayerSpentManaPoints < MinManaSpent)
                    MinManaSpent = currentTurn.PlayerSpentManaPoints;

                return currentTurn;
            }

            // Operate turn
            // Step 2 - Perform action
            if (currentAction == Action.EnemyAttack)
            {
                int armor = (shieldIsActive) ? ShieldArmorIncrease : 0;
                currentTurn.PlayerHitPoints -= Math.Max(1, EnemyDamage - armor);
            }
            else
            {
                currentTurn.EnemyHitPoints -= Damage(currentAction);
                currentTurn.PlayerCurrentManaPoints -= Cost(currentAction);
                currentTurn.PlayerSpentManaPoints += Cost(currentAction);

                if (currentAction == Action.Drain)
                    currentTurn.PlayerHitPoints += 2;

                var timer = Timer(currentAction);
                if (timer > 0)
                {
                    if (currentAction == Action.Shield)
                        currentTurn.activeEffect_Shield = timer;
                    if (currentAction == Action.Poison)
                        currentTurn.activeEffect_Poison = timer;
                    if (currentAction == Action.Recharge)
                        currentTurn.activeEffect_Recharge = timer;
                }
            }

            // Game over conditions
            if (currentTurn.PlayerHitPoints <= 0)
            {
                currentTurn.GameEnded = true;
                currentTurn.PlayerVictory = false;
                currentTurn.outcome = EndCondition.EnemyVictory;
                finalRounds.Add(currentTurn);
                return currentTurn;
            }
            if (currentTurn.EnemyHitPoints <= 0)
            {
                currentTurn.GameEnded = true;
                currentTurn.PlayerVictory = true;
                currentTurn.outcome = EndCondition.PlayerVictory;
                finalRounds.Add(currentTurn);
                if (currentTurn.PlayerSpentManaPoints < MinManaSpent)
                    MinManaSpent = currentTurn.PlayerSpentManaPoints;
                return currentTurn;
            }
            if (currentTurn.PlayerCurrentManaPoints < 0)
            {
                currentTurn.GameEnded = true;
                currentTurn.PlayerVictory = false;
                currentTurn.outcome = EndCondition.RanOutOfMana;
                finalRounds.Add(currentTurn);
                return currentTurn;
            }

            // Determine next actions

            List<Action> possibleActions = new();

            if (currentTurn.PlayerTurn)
                possibleActions.Add(Action.EnemyAttack);
            else
            {
                var nextTurnMana = currentTurn.PlayerCurrentManaPoints + ((currentTurn.activeEffect_Recharge > 0) ? RechargeManaIncrease : 0);
                possibleActions = AllPlayerActions.Where(x => Cost(x) <= nextTurnMana).ToList();
                if (currentTurn.activeEffect_Poison > 1)
                    possibleActions.Remove(Action.Poison);
                if (currentTurn.activeEffect_Shield > 1)
                    possibleActions.Remove(Action.Shield);
                if (currentTurn.activeEffect_Recharge > 1)
                    possibleActions.Remove(Action.Recharge);
            }
            

            if (possibleActions.Count == 0)
            {
                // Next turn will be player's turn and no action can be casted
                currentTurn.GameEnded = true;
                currentTurn.PlayerVictory = false;
                currentTurn.outcome = EndCondition.CannotCastAction;
                finalRounds.Add(currentTurn);
                return currentTurn;
            }

            currentTurn.children = new();

            foreach (var nextAction in possibleActions)
                currentTurn.children.Add(NextTurn(currentTurn, nextAction));

            return currentTurn;
        }

        public void GeneratePossibleGames(List<Turn> game, Turn lastTurn, int part=1)
        {
           // if (part == 1 && lastTurn.PlayerSpentManaPoints > MinManaSpent)
           //     return;

            Turn newTurn = lastTurn.Clone();
            
            newTurn.PlayerTurn = !lastTurn.PlayerTurn;

            newTurn.AdvanceEffects(lastTurn);
            bool shieldIsActive = false;

            // 1. Apply effects in place
            if(newTurn.activeEffect_Shield > 0)
                shieldIsActive = true;
            
            if (newTurn.activeEffect_Recharge > 0)
                newTurn.PlayerCurrentManaPoints += RechargeManaIncrease;

            if (newTurn.activeEffect_Poison > 0)
                newTurn.EnemyHitPoints -= PoisonTurnDamage;
            

            // 1a. Check endgame (poison kill)
            if (newTurn.EnemyHitPoints <= 0)
            {
                newTurn.GameEnded = true;
                newTurn.PlayerVictory = true;
                game.Add(newTurn);
                possibleGames.Add(game);
                finalRounds.Add(newTurn);
                if (newTurn.PlayerSpentManaPoints < MinManaSpent)
                    MinManaSpent = newTurn.PlayerSpentManaPoints;
                return;
            }

            if (!newTurn.PlayerTurn)
            {
                // Enemy turn
                // Consider effects in place
                int armor = (shieldIsActive) ? ShieldArmorIncrease : 0;
                newTurn.action = Action.EnemyAttack;
                newTurn.PlayerHitPoints -= Math.Max(1, EnemyDamage - armor);
                
                
                if (newTurn.PlayerHitPoints <= 0)
                {
                    // Check Endgame (Player defeated)
                    newTurn.GameEnded = true;
                    newTurn.PlayerVictory = false;
                    game.Add(newTurn);
                    possibleGames.Add(game);
                    finalRounds.Add(newTurn);
                    return;
                }

                // Keep recursing
                GeneratePossibleGames(game, newTurn);
            }
            else
            {
                // Player Turn

                List<Action> invalidActions = new();        // Get the current active effects 
                if (newTurn.activeEffect_Poison > 0)
                    invalidActions.Add(Action.Poison);
                if (newTurn.activeEffect_Shield > 0)
                    invalidActions.Add(Action.Shield);
                if(newTurn.activeEffect_Recharge>0)
                    invalidActions.Add(Action.Recharge);

                List<Action> possibleActions = new();

                foreach (var action in AllPlayerActions)
                    if (!invalidActions.Contains(action))
                        possibleActions.Add(action);

                possibleActions = possibleActions.Where(x => Cost(x) < newTurn.PlayerCurrentManaPoints).ToList();

                if (possibleActions.Count == 0)
                {
                    // Check Endgame (Cannot cast action)
                    newTurn.GameEnded = true;
                    newTurn.PlayerVictory = false;
                    game.Add(newTurn);
                    possibleGames.Add(game);
                    finalRounds.Add(newTurn);
                    return;
                }

                foreach (var action in possibleActions)
                {
                    var newPlayerTurn = newTurn.Clone();
                    var newGame = new List<Turn>();

                    foreach (var t in game)
                        newGame.Add(t.Clone());

                    newPlayerTurn.action = action;
                    // Solve damage
                    newPlayerTurn.EnemyHitPoints -= Damage(action);
                    // Solve cost
                    newPlayerTurn.PlayerCurrentManaPoints -= Cost(action);
                    newPlayerTurn.PlayerSpentManaPoints += Cost(action);

                    if (action == Action.Drain)
                        newPlayerTurn.PlayerHitPoints += 2;

                    // Add actions to the turn with starting timer
                    var timer = Timer(action);
                    if (timer > 0)
                    {
                        if (action == Action.Shield)
                            newPlayerTurn.activeEffect_Shield = timer;
                        if (action == Action.Poison)
                            newPlayerTurn.activeEffect_Poison = timer;
                        if (action == Action.Recharge)
                            newPlayerTurn.activeEffect_Recharge = timer;

                    }
                    

                    if (newPlayerTurn.PlayerCurrentManaPoints <= 0)
                    {
                        // Check Endgame - Enemy defeated
                        newPlayerTurn.GameEnded = true;
                        newPlayerTurn.PlayerVictory = false;
                        newGame.Add(newPlayerTurn);
                        possibleGames.Add(newGame);
                        finalRounds.Add(newPlayerTurn);
                        return;
                    }

                    if (newPlayerTurn.EnemyHitPoints <= 0)
                    {
                        // Check Endgame - Enemy defeated
                        newPlayerTurn.GameEnded = true;
                        newPlayerTurn.PlayerVictory = true;
                        newGame.Add(newPlayerTurn);
                        possibleGames.Add(newGame);
                        finalRounds.Add(newPlayerTurn);
                        if (newPlayerTurn.PlayerSpentManaPoints < MinManaSpent)
                            MinManaSpent = newPlayerTurn.PlayerSpentManaPoints;
                        return;
                    }
                    
                    newGame.Add(newPlayerTurn);
                    // Keep recursing
                    GeneratePossibleGames(newGame, newPlayerTurn);
                }
            }
        }

        public void ParseInput(List<string> lines)
        {
            EnemyHitPoints = int.Parse(lines[0].Replace("Hit Points: ", ""));
            EnemyDamage = int.Parse(lines[1].Replace("Damage: ", ""));
        }

        int LessManaVictory()
        {
            possibleGames.Clear();
            finalRounds.Clear();

            Turn startingTurn = new();
            List<Turn> startingList = new();
            startingTurn.PlayerTurn = true;
            startingTurn.PlayerHitPoints = 50 + EnemyDamage;
            startingTurn.PlayerCurrentManaPoints = 500;
            startingTurn.EnemyHitPoints = EnemyHitPoints;
            startingTurn.EnemyDamage = EnemyDamage;

            //GeneratePossibleGames(startingList, startingTurn);
            startingTurn.children = new()
            {
                NextTurn(startingTurn, Action.EnemyAttack)
            };

            var victories = finalRounds.Where(x => x.PlayerVictory == true).ToList();
            var min = victories.Min(t => t.PlayerSpentManaPoints);
            var game = finalRounds.Where(t => t.PlayerSpentManaPoints == min && (t.outcome == EndCondition.PlayerVictory || t.outcome == EndCondition.PlayerVictory_Poison)).FirstOrDefault();
            var currentTurn = game;
            while (currentTurn != null)
            {
                Trace.WriteLine(currentTurn.OneLiner());
                currentTurn = currentTurn.parent;
            }



            return min;
        }


        public int Solve(int part = 1)
            => (part == 1) ? LessManaVictory() : 0;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// Since I approached Day 21 problem creating all possible setups in lists, Day 22 problem will be implemented as a tree, where
// each turn will have a collection of children turns that will be the possible actions that can go next. 
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

    class Turn
    {
        public bool PlayerTurn;
        public Action action;

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


        public Turn NextTurn(Turn previousTurn, Action currentAction, int part = 1)
        {
            Turn currentTurn = previousTurn.Clone();
            currentTurn.PlayerTurn = !previousTurn.PlayerTurn;
            currentTurn.action = currentAction;
            currentTurn.parent = previousTurn;

            if (part == 2)
            {
                currentTurn.PlayerHitPoints -= 1;

                if (currentTurn.PlayerHitPoints <= 0)
                {
                    currentTurn.GameEnded = true;
                    currentTurn.PlayerVictory = false;
                    finalRounds.Add(currentTurn);
                    return currentTurn;
                }
            }

            if (currentTurn.PlayerSpentManaPoints > MinManaSpent)
            {
                currentTurn.PlayerVictory = false;
                currentTurn.GameEnded = true;
                finalRounds.Add(currentTurn);
                return currentTurn;
            }

            // 1. Apply effects in place and advance timer
            bool shieldIsActive = (currentTurn.activeEffect_Shield > 0);
            currentTurn.PlayerCurrentManaPoints += (currentTurn.activeEffect_Recharge > 0) ? RechargeManaIncrease : 0;
            currentTurn.EnemyHitPoints -= (currentTurn.activeEffect_Poison > 0) ? PoisonTurnDamage : 0;
            currentTurn.AdvanceEffects();

            // Check game over condition 1 - Poison kill
            if (currentTurn.EnemyHitPoints <= 0)
            {
                currentTurn.GameEnded = true;
                currentTurn.PlayerVictory = true;
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
            if ( currentTurn.PlayerHitPoints <= 0 || currentTurn.PlayerCurrentManaPoints < 0)
            {
                currentTurn.GameEnded = true;
                currentTurn.PlayerVictory = false;
                finalRounds.Add(currentTurn);
                return currentTurn;
            }
            if (currentTurn.EnemyHitPoints <= 0)
            {
                currentTurn.GameEnded = true;
                currentTurn.PlayerVictory = true;
                
                finalRounds.Add(currentTurn);
                if (currentTurn.PlayerSpentManaPoints < MinManaSpent)
                    MinManaSpent = currentTurn.PlayerSpentManaPoints;
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
                finalRounds.Add(currentTurn);
                return currentTurn;
            }

            currentTurn.children = new();

            foreach (var nextAction in possibleActions)
                currentTurn.children.Add(NextTurn(currentTurn, nextAction, part));

            return currentTurn;
        }

        public void ParseInput(List<string> lines)
        {
            EnemyHitPoints = int.Parse(lines[0].Replace("Hit Points: ", ""));
            EnemyDamage = int.Parse(lines[1].Replace("Damage: ", ""));
        }

        int LessManaVictory(int part = 1)
        {
            finalRounds.Clear();

            // The starting point will be an enemy turn where the player will have the health + damage as starting hitpoints
            // That way we can generate the possibles games tree from a single node
            Turn startingTurn = new();
            List<Turn> startingList = new();

            startingTurn.PlayerTurn = true;
            startingTurn.PlayerHitPoints = 50 + EnemyDamage + (part == 2 ? 1 : 0);
            startingTurn.PlayerCurrentManaPoints = 500;
            startingTurn.EnemyHitPoints = EnemyHitPoints;
            startingTurn.EnemyDamage = EnemyDamage;

            startingTurn.children = new()
            {
                NextTurn(startingTurn, Action.EnemyAttack, part)
            };

            return finalRounds.Where(x => x.PlayerVictory == true).Min(t => t.PlayerSpentManaPoints);
        }


        public int Solve(int part = 1)
            => LessManaVictory(part);
    }
}

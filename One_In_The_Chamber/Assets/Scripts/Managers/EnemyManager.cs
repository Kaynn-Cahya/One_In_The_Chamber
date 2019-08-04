using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MyBox;

public class EnemyManager : Singleton<EnemyManager> {

    [SerializeField, Tooltip("The player in the scene"), MustBeAssigned]
    private Player player;

    private HashSet<Enemy> activeEnemies;

    private void Awake() {
        activeEnemies = new HashSet<Enemy>();
        EnemySpawner.Instance.onEnemySpawnedEvent += HandleNewEnemySpawned;
    }

    private void HandleNewEnemySpawned(Enemy newEnemy) {
        newEnemy.InitalizeEnemy(player.transform);

        activeEnemies.Add(newEnemy);
        RemoveEnemyFromActiveEnemiesOnEnemyDeath();

        #region Local_Function

        void RemoveEnemyFromActiveEnemiesOnEnemyDeath() {
            newEnemy.onCharacterDeathEvent += delegate {
                activeEnemies.Remove(newEnemy);
            };
        }

        #endregion
    }

    public T FetchEnemiesByCondition<T>(Func<Enemy, bool> condition) where T : ICollection<Enemy>  {

        ICollection<Enemy> result = new HashSet<Enemy>();

        foreach (var activeEnemy in activeEnemies) {
            if (condition(activeEnemy)) {
                result.Add(activeEnemy);
            }
        }

        return (T) result;
    }
}

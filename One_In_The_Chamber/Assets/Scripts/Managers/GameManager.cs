using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MyBox;

public class GameManager : Singleton<GameManager> {

    public delegate void OnPlayerReachedImprovementPoint(GunType improvedGunType);

    /// <summary>
    /// Invoked when the player gun's got improved.
    /// </summary>
    public OnPlayerReachedImprovementPoint onPlayerReachedImprovementPointEvent;

    #region PlayerImprovementPoint_Struct

    [System.Serializable]
    private class PlayerImprovementPoint {
        [SerializeField, Tooltip("What gun type to switch to for the player at this improvement point"), SearchableEnum]
        private GunType improvementPointGunType;

        [SerializeField, Tooltip("How many enemies to defeat to reach this improvement point"), PositiveValueOnly]
        private int enemiesToDefeatCount;

        public GunType ImprovementPointGunType { get => improvementPointGunType; }
        public int EnemiesToDefeatCount { get => enemiesToDefeatCount; }

        public bool Reached { get; set; }
    }

    #endregion

    [SerializeField, Tooltip("The improvement points for the player to upgrade guns"), MustBeAssigned]
    private PlayerImprovementPoint[] playerImprovementPoints;

    [SerializeField, Tooltip("The player in the scene"), MustBeAssigned]
    private Player playerChar;

    public int EnemiesDefeated { get; private set; }

    void Awake() {
        EnemiesDefeated = 0;
        playerChar.onCharacterDeathEvent += OnPlayerDeath;
        SetupPlayerImprovementPoints();

        #region Local_Function

        void SetupPlayerImprovementPoints() {
            foreach (var playerImprovementPoint in playerImprovementPoints) {
                playerImprovementPoint.Reached = false;
            }
        }

        #endregion
    }

    public void TriggerEnemyFellOffArena() {
        // TODO: If you wanted some effects to occur when the enemy falls off the arena.
        ++EnemiesDefeated;

        UpdatePlayerImprovementPoints();
    }

    private void UpdatePlayerImprovementPoints() {
        foreach (var playerImprovementPoint in playerImprovementPoints) {
            if (playerImprovementPoint.Reached) { continue; }

            if (playerImprovementPoint.EnemiesToDefeatCount <= EnemiesDefeated) {
                TriggerPlayerReachedImprovementPoint(playerImprovementPoint);
                break;
            }
        }

        #region Local_Function

        void TriggerPlayerReachedImprovementPoint(PlayerImprovementPoint playerImprovementPoint) {
            playerImprovementPoint.Reached = true;
            playerChar.SwitchToLoadoutOfGunType(playerImprovementPoint.ImprovementPointGunType);
            onPlayerReachedImprovementPointEvent?.Invoke(playerImprovementPoint.ImprovementPointGunType);
        }

        #endregion
    }

    private void OnPlayerDeath() {
        // TODO: Do something when the player falls off the arena.
    }
}

public class AutoEnemy : Enemy
{
    protected override void OnDestinationReached() {
        this.targetGridCoordinates = EnemyGrid.current.GetFarPointCoordinates(this.targetGridCoordinates);
    }
}
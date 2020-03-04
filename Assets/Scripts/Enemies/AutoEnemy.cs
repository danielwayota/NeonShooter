public class AutoEnemy : Enemy
{
    protected override void OnDestinationReached() {
        this.targetPosition = EnemyGrid.current.GetFarPoint(this.targetPosition, this.minDistanceToPoint);
    }
}
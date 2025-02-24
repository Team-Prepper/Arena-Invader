namespace BoardGame
{
    public class OneMoreAndBackHome : IOverlapEvent
    {

        public void Event(Plate plate, Pawn newPawn)
        {
            Pawn defaultPawn = plate.GetPawn();

            defaultPawn.BackHome();
            SFXManager.Instance.PlaySFX("Kill");

            //Instantiate(_catchEffect, transform.position + Vector3.up, Quaternion.identity);

            plate.SetPawn(newPawn);
            newPawn.GetPlayer().AddChance();
        }

    }
}
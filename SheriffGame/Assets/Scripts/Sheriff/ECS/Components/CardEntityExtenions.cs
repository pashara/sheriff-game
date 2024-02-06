namespace Sheriff.ECS.Components
{
    public static class CardEntityExtenions
    {
        public static void MarkReleased(this CardEntity cardEntity)
        {
            cardEntity.isSelectToDeclare = false;
            cardEntity.isInDec = false;
            cardEntity.isCardOnHand = false;
            cardEntity.isCardRelease = true;
        }
        
        public static void PutInDec(this CardEntity cardEntity)
        {
            UnlinkOwner(cardEntity);
            cardEntity.isSelectToDeclare = false;
            cardEntity.isInDec = true;
            cardEntity.isCardOnHand = false;
            cardEntity.isCardRelease = false;
        }
        
        
        
        public static void MarkOnHand(this CardEntity cardEntity, PlayerEntityId playerEntityId)
        {
            cardEntity.ReplaceCardOwner(playerEntityId);
            cardEntity.isSelectToDeclare = false;
            cardEntity.isInDec = false;
            cardEntity.isCardOnHand = true;
            cardEntity.isCardRelease = false;
        }
        
        
        public static void MarkSelected(this CardEntity cardEntity)
        {
            cardEntity.isSelectToDeclare = true;
            cardEntity.isInDec = false;
            cardEntity.isCardOnHand = false;
            cardEntity.isCardRelease = false;
        }

        public static void UnlinkOwner(this CardEntity cardEntity)
        {
            if (cardEntity.hasCardOwner)
                cardEntity.RemoveCardOwner();
        }
        
        
    }
}
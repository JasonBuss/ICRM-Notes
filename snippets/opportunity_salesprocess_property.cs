    public static partial class OpportunityBusinessRules
    {
        public static void GetSalesProcessStep( IOpportunity opportunity, out System.String result)
        {
		
			Sage.Platform.RepositoryHelper<Sage.Entity.Interfaces.ISalesProcesses> repository = Sage.Platform.EntityFactory.GetRepositoryHelper<Sage.Entity.Interfaces.ISalesProcesses>();
	 		Sage.Platform.Repository.ICriteria criteria = repository.CreateCriteria();
			criteria.Add(repository.EF.Eq("EntityId", opportunity.Id.ToString()));
			criteria.AddOrder(repository.EF.Desc("ModifyDate"));
			var processes = criteria.List<Sage.Entity.Interfaces.ISalesProcesses>();
			
			string spName = "";
			
			foreach (Sage.Entity.Interfaces.ISalesProcesses sp in processes)
			{
				spName = sp.Name;
				break;
			}
			
			result = spName;
        }
    }
    
	//Client wanted to display the Opportunity Salesprocess on the Account and Contact opportunity tabs.
	//Originally, I tried creating a relationship from Opportunity to the SalesProcesses entity.  This
	//was an issue because there could be more than one record in the Salesprocesses table with an entityid for 
	//a given opportunity.
	
	//Instead, I've created a code snippet property in opportunity to display the most recent salesprocesses record 
	//for the opportunity.
	
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
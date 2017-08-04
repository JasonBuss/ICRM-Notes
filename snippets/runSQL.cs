	//Running SQL Statment in BR

	
	//Using OLECommand object to return data.
	
	string pluginID = string.Empty;
	Sage.Platform.Data.IDataService datasvc = Sage.Platform.Application.ApplicationContext.Current.Services.Get<Sage.Platform.Data.IDataService>();
	using (System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection(datasvc.GetConnectionString()))
	{
		conn.Open();
		using (System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand("select pluginid from plugin where basedon is not null and name='HMC' and type=1 and family='Sales'", conn))
		{
			object o = cmd.ExecuteScalar();
			if (o != null) pluginID = o.ToString();
		}
	}						
	
	
	//DEFINE SQL and Run
	string sql = "select stageorder from salesprocessaudit ";
	sql += "where ENTITYID='{0}' and PROCESSTYPE='Stage' and stagename='{1}'";
	
	string sql2 = "update salesprocessaudit set ";
	sql2 += "COMPLETED='T', ";
	sql2 += "COMPLETEDDATE=GETUTCDATE(), ";
	sql2 += "COMPLETEDBY='{0}' ";
	sql2 += "where ENTITYID='{1}' and STAGEORDER<{2} and ProcessType in ('Stage','Step')";
	
	string sql3 = "update SALESPROCESSAUDIT set ";
	sql3 += "STARTDATE=GETUTCDATE(), ";
	sql3 += "STARTEDBY='{0}' ";
	sql3 += "where ENTITYID='{1}' and STAGEORDER={2} and ProcessType = 'Stage'";
	
	string sql4 = "update salesprocessaudit set iscurrent = 'F' where ENTITYID='{0}'"; 
	string sql5 = "update salesprocessaudit set iscurrent = 'T' where ENTITYID='{0}' and STAGENAME='{1}'";
	
	string stageNumber = string.Empty;
	
	using (System.Data.OleDb.OleDbConnection conn2 = new System.Data.OleDb.OleDbConnection(datasvc.GetConnectionString()))
	{				
		conn2.Open();
		using (System.Data.OleDb.OleDbCommand cmd2 = new System.Data.OleDb.OleDbCommand(
			string.Format(
						sql,
						opportunity.Id.ToString(), 
						plainStage
			), 
			conn2
		))
		{
			object o = cmd2.ExecuteScalar();
			if (o != null) stageNumber = o.ToString();
		}								
		
		if(string.IsNullOrEmpty(stageNumber)) return;
						
		using (System.Data.OleDb.OleDbCommand cmd3 = new System.Data.OleDb.OleDbCommand(
			string.Format(					
					sql2, 
					Sage.SalesLogix.API.MySlx.Security.CurrentSalesLogixUser.Id,
					opportunity.Id.ToString(), 
					stageNumber), 
			conn2
		))
		{
			object o = cmd3.ExecuteScalar();		            
		}												
		using (System.Data.OleDb.OleDbCommand cmd4 = new System.Data.OleDb.OleDbCommand(
			string.Format(
					sql3, 
					Sage.SalesLogix.API.MySlx.Security.CurrentSalesLogixUser.Id,
					opportunity.Id.ToString(), 
					stageNumber), 
			conn2
		))
		{
			object o = cmd4.ExecuteScalar();		            
		}	
		
		using (System.Data.OleDb.OleDbCommand cmd5 = new System.Data.OleDb.OleDbCommand(
			string.Format(
					sql4, 							
					opportunity.Id.ToString()), 
			conn2
		))
		{
			object o = cmd5.ExecuteScalar();		            
		}	
		
		using (System.Data.OleDb.OleDbCommand cmd6 = new System.Data.OleDb.OleDbCommand(
			string.Format(
					sql5, 							
					opportunity.Id.ToString(),
					plainStage
			), 
			conn2
		))
		{
			object o = cmd6.ExecuteScalar();		            
		}	
	}
								
	opportunity.Stage = stage;			
	opportunity.Save();			
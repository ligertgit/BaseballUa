using BaseballUa.BlData;
using BaseballUa.Data;
using BaseballUa.Models;
using BaseballUa.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BaseballUa.DTO
{
    public class SchemaGroupToView
    {
        private readonly BaseballUaDbContext _dbContext;

        public SchemaGroupToView(BaseballUaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public SchemaGroupViewModel Convert(SchemaGroup schemaGroupDAL)
        {
            var schemaGroupVL = new SchemaGroupViewModel();
            schemaGroupVL.Id = schemaGroupDAL.Id;
            schemaGroupVL.GroupName = schemaGroupDAL.GroupName;
            schemaGroupVL.EventSchemaItemId = schemaGroupDAL.EventSchemaItemId;
            schemaGroupVL.EventSchemaItem = schemaGroupDAL.EventSchemaItem;
            if (schemaGroupVL.EventSchemaItem == null)
            {
                schemaGroupVL.EventSchemaItem = new EventSchemaItemsCrud(_dbContext).Get(schemaGroupDAL.EventSchemaItemId);
            }
            schemaGroupVL.EventSchemaItems = new EventSchemaItemsCrud(_dbContext).GetEventSchemaItems(schemaGroupVL.EventSchemaItem.EventId)
                                                    .Select(i => new SelectListItem
                                                                    {
                                                                        Value = i.Id.ToString(),
                                                                        Text = i.SchemaItem.ToString()
                                                                    }
                                                    ).ToList();
            schemaGroupVL.Games = new GameToView().ConvertAll(schemaGroupDAL.Games?.ToList());
            schemaGroupVL.VirtualTeams = new List<TeamViewModel>();
            // get uniq teams for group
            if (schemaGroupVL.Games.Count > 0) 
            { 
                List<TeamViewModel> groupTeams = new List<TeamViewModel>();
                foreach(var game in schemaGroupVL.Games)
                {
                    if (game.HomeTeamId != null)
                    {
                        groupTeams.Add(game.HomeTeam);
                    }
                    if (game.VisitorTeamId != null) 
                    {
                        groupTeams.Add(game.VisitorTeam);
                    }
                    
                }
                schemaGroupVL.VirtualTeams = groupTeams.GroupBy(t => t.Id).Select(g => g.First()).ToList(); ;
            }

            return schemaGroupVL;
        }

        public List<SchemaGroupViewModel> ConvertAll(List<SchemaGroup> schemaGroupsDAL) 
        { 
            var schemaGroupsVL = new List<SchemaGroupViewModel>();
            foreach (var schemaGroupDAL in schemaGroupsDAL) 
            {
                schemaGroupsVL.Add(Convert(schemaGroupDAL));
            }

            return schemaGroupsVL;
        }

        public SchemaGroupViewModel CreateEmpty(int eventSchemaItemId)
        {
            var schemaGroupVL = new SchemaGroupViewModel();
            schemaGroupVL.EventSchemaItemId = eventSchemaItemId;
            schemaGroupVL.GroupName = string.Empty;
            schemaGroupVL.EventSchemaItem = new EventSchemaItemsCrud(_dbContext).Get(eventSchemaItemId);
            //schemaGroupVL.EventSchemaItems = new EventSchemaItemsCrud(_dbContext).GetEventSchemaItems(eventSchemaItemId)
            //                                        .Select(i => new SelectListItem
            //                                        {
            //                                            Value = i.Id.ToString(),
            //                                            Text = i.SchemaItem.ToString()
            //                                        }
            //                                        ).ToList();

            return schemaGroupVL;
        }

        public SchemaGroup ConvertBack(SchemaGroupViewModel schemaGroupVL)
        {
            var schemaGroupDAL = new SchemaGroup();
            schemaGroupDAL.Id = schemaGroupVL.Id;
            schemaGroupDAL.GroupName = schemaGroupVL.GroupName;
            schemaGroupDAL.EventSchemaItemId = schemaGroupVL.EventSchemaItemId;

            return schemaGroupDAL;
        }
    }
}

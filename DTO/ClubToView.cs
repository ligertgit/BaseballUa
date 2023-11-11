using BaseballUa.Data;
using BaseballUa.Models;
using BaseballUa.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;

namespace BaseballUa.DTO
{
    public class ClubToView
    {
        //private readonly BaseballUaDbContext _dbContext;

        //public ClubToView(BaseballUaDbContext dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        public ClubViewModel Convert(Club clubDAL, bool doSubConvert = true)
        {
            var clubVL = new ClubViewModel();
            clubVL.Id = clubDAL.Id;
            clubVL.Name = clubDAL.Name;
            clubVL.Description = clubDAL.Description;
            clubVL.Invitation = clubDAL.Invitation;
            clubVL.FnameLogoSmall = clubDAL.FnameLogoSmall;
            clubVL.FnameLogoBig = clubDAL.FnameLogoBig;
            clubVL.CountryId = clubDAL.CountryId;
            if (clubDAL.Country != null) 
            {
				clubVL.Country = new CountryToView().Convert(clubDAL.Country, false);
			}
            
            //clubVL.CountriesList = _dbContext.Countries.Select(c => new SelectListItem
            //                            {
            //                                Value = c.Id.ToString(),
            //                                Text = c.Name
            //                            });
            if (clubDAL.Teams != null && doSubConvert) 
            {
                clubVL.Teams = new TeamToView().ConvertAll(clubDAL.Teams.ToList(), false);
            }
            if (clubDAL.Staffs != null && doSubConvert) 
            {
                clubVL.Staffs = new StaffToView().ConvertAll(clubDAL.Staffs.ToList(), false);
            }

            return clubVL;
        }

        public List<ClubViewModel> ConvertAll(List<Club> clubsDAL, bool doSubConvert = true) 
        {
            var clubsVL = new List<ClubViewModel>();
            foreach (var clubDAL in clubsDAL)
            {
                clubsVL.Add(Convert(clubDAL, doSubConvert));
            }

            return clubsVL;
        }

        public Club ConvertBack(ClubViewModel clubVL)
        {
            var clubDAL = new Club();
            clubDAL.Id = clubVL.Id;
            clubDAL.Name = clubVL.Name;
            clubDAL.Description = clubVL.Description;
            clubDAL.Invitation = clubVL.Invitation;
            clubDAL.FnameLogoSmall = clubVL.FnameLogoSmall;
            clubDAL.FnameLogoBig = clubVL.FnameLogoBig;
            clubDAL.CountryId = clubVL.CountryId;

            return clubDAL;
        }

        public ClubViewModel CreateEmpty()
        { 
            var clubVL = new ClubViewModel();
            
            return clubVL;
        }
    }
}

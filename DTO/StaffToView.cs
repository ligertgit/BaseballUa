using BaseballUa.Data;
using BaseballUa.Models;
using BaseballUa.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace BaseballUa.DTO
{
    public class StaffToView
    {
        public StaffViewModel Convert(Staff staffDAL, bool doSubConvert = true)
        {
            var staffVL = new StaffViewModel();
            staffVL.Id = staffDAL.Id;
            staffVL.FirsName = staffDAL.FirsName;
            staffVL.SecondName = staffDAL.SecondName;
            staffVL.Role = staffDAL.Role;
            staffVL.RoleDescription = staffDAL.RoleDescription;
            staffVL.AvatarSmall = staffDAL.AvatarSmall;
            staffVL.AvatarLarge = staffDAL.AvatarLarge;
            staffVL.ClubId = staffDAL.ClubId;
            if (staffDAL.Club != null) 
            { 
                staffVL.Club = new ClubToView().Convert(staffDAL.Club, false);
            }

            return staffVL;
        }

        public List<StaffViewModel> ConvertAll(List<Staff> staffsDAL, bool doSubConvert = true) 
        { 
            List<StaffViewModel> staffsVL = new List<StaffViewModel>();
            foreach (var staffDAL in staffsDAL)
            {
                staffsVL.Add(Convert(staffDAL, doSubConvert));
            }

            return staffsVL;
        }

        public StaffViewModel CreateEmpty(int clubId)
        {
            var staffVL = new StaffViewModel();
            staffVL.ClubId = clubId;

            return staffVL;
        }

        public Staff ConvertBack(StaffViewModel staffVL) 
        {
            var staffDAL = new Staff();

            staffDAL.Id = staffVL.Id;
            staffDAL.FirsName = staffVL.FirsName;
            staffDAL.SecondName = staffVL.SecondName;
            staffDAL.Role = staffVL.Role;
            staffDAL.RoleDescription = staffVL.RoleDescription;
            staffDAL.AvatarSmall = staffVL.AvatarSmall == null ? Constants.DefaultStaffSmallImage : staffVL.AvatarSmall;
            staffDAL.AvatarLarge = staffVL.AvatarLarge == null ? Constants.DefaultStaffBigImage : staffVL.AvatarLarge; ;
            staffDAL.ClubId = staffVL.ClubId;

            return staffDAL;
        } 


    }
}

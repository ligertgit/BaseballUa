using Microsoft.AspNetCore.Mvc.Rendering;

namespace BaseballUa.ViewModels.Custom
{
    public class ShowAlbums
    {
        public List<AlbumVM> Albums { get; set; } = new List<AlbumVM>();

        public ShowAlbumsSelections Selections { get; set; } = new ShowAlbumsSelections();
    }
}

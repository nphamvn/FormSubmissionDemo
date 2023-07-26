using FormSubmissionDemo.Data.Repositories;
using FormSubmissionDemo.Entities;
using FormSubmissionDemo.Models.Users;
using FormSubmissionDemo.Models.Shared;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormSubmissionDemo.Services;

public interface IUserService
{
    Task<UserIndexViewModel> GetUserIndexViewModel();
    Task<UserModel> GetUserModel(int userId);
    Task<UserSaveViewModel> GetCreateViewModel();
    Task ValidateCreate(UserSaveViewModel userSaveViewModel, ModelStateDictionary modelState);
    Task<int> Create(UserSaveViewModel model);
    Task<UserSaveViewModel> GetEditViewModel(int userId);
    Task ValidateEdit(int userId, UserSaveViewModel userSaveViewModel, ModelStateDictionary modelState);
    Task Edit(int userId, UserSaveViewModel userSaveViewModel);
}
public class UserService : BaseService, IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IAppFileService appFileService
        , ITempFileService tempFileService
        , IUserRepository userRepository) : base(appFileService, tempFileService)
    {
        _userRepository = userRepository;
    }

    public async Task<UserModel> GetUserModel(int userId)
    {
        var entity = await _userRepository.Get(userId);
        return new ()
        {
            UserId  = entity.UserId,
            Username = entity.Username,
            ProfileImageFileId = entity.ProfileFileId
        };
    }

    public async Task<UserSaveViewModel> GetCreateViewModel()
    {
        var vm = new UserSaveViewModel
        {
            //FavoriteColorSelectListItems = GetFavoriteColorSelectListItems(),
            // AddressCountrySelectListItems = GetAddressCountrySelectListItems(),
            // AddressStateProvinceSelectListItems = GetCountryStateProvinceSelectListItems(model.Address?.CountryId ?? 0),
            // ProfileItems = GetProfileItems(model.ProfileIds),
            // TagfifyTagWhiteList = GetWhiteListTags()
        };
        return vm;
    }

    public async Task ValidateCreate(UserSaveViewModel userSaveViewModel, ModelStateDictionary modelState)
    {
        if(!modelState.IsValid) return;
        //TODO: Do validation
    }

    public async Task<int> Create(UserSaveViewModel model)
    {
        var id = await _userRepository.Add(new UserEntity()
        {
            Username = model.Username,
            ProfileFileId = model.ProfilePicture.AppFileId.Value
        });
        return id;
    }

    public async Task<UserSaveViewModel> GetEditViewModel(int userId)
    {
        var entity = await _userRepository.Get(userId);
        var vm = new UserSaveViewModel
        {
            Username = entity.Username,
            //FavoriteColorSelectListItems = GetFavoriteColorSelectListItems(),
            // AddressCountrySelectListItems = GetAddressCountrySelectListItems(),
            // AddressStateProvinceSelectListItems = GetCountryStateProvinceSelectListItems(model.Address?.CountryId ?? 0),
            // ProfileItems = GetProfileItems(model.ProfileIds),
            // TagfifyTagWhiteList = GetWhiteListTags()
        };
        return vm;
    }

    public async Task ValidateEdit(int userId, UserSaveViewModel userSaveViewModel, ModelStateDictionary modelState)
    {
        if(!modelState.IsValid) return;
        //TODO: Do validation
    }

    public async Task Edit(int userId, UserSaveViewModel model)
    {
        await _userRepository.Update(userId, new UserEntity()
        {
            Username = model.Username
        });
    }
    

    public async Task<UserIndexViewModel> GetUserIndexViewModel()
    {
        var userEntities = await _userRepository.GetAll();
        var vm = new UserIndexViewModel
        {
            Users = PaginatedList<UserModel>.Create(userEntities.Select(ue => new UserModel {
                
            }).AsQueryable(), 0, 10)
        };
        return vm;
    }
    
    private List<SelectListItem> GetWhiteListTags()
    {
        return Enumerable.Range(1, 20).Select(i => new SelectListItem() {
            Text = $"Tag {i}",
            Value = i.ToString()
        }).ToList();
    }

    // private List<UserSaveViewModel.ProfileItem> GetProfileItems(List<int> profileIds)
    // {
    //     return profileIds.Select(id => new UserSaveViewModel.ProfileItem()
    //     {
    //         Id = id,
    //         Name = $"Profile {id}"
    //     }).ToList();
    // }

    // private List<SelectListItem> GetFavoriteColorSelectListItems()
    // {
    //     return new()
    //     {
    //         new("Red", "red"),
    //         new("Indigo", "indigo"),
    //         new("Orange", "orange")
    //     };
    // }
}
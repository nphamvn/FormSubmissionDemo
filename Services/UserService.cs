using FormSubmissionDemo.Data.Repositories;
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
    Task<int> Create(UserSaveViewModel userSaveViewModel);
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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public Task<int> Create(UserSaveViewModel userSaveViewModel)
    {
        throw new NotImplementedException();
    }

    public Task<UserSaveViewModel> GetEditViewModel(int userId)
    {
        throw new NotImplementedException();
    }

    public async Task ValidateEdit(int userId, UserSaveViewModel userSaveViewModel, ModelStateDictionary modelState)
    {
        throw new NotImplementedException();
    }

    public async Task Edit(int userId, UserSaveViewModel userSaveViewModel)
    {
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
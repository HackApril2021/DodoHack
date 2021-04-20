using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Infrastructure.Verbatims;
using Models.Db.Account;
using Models.Dtos;
using Models.DTOs;
using Models.DTOs.Misc;
using Models.DTOs.Responses;
using Models.DTOs.WorkerAccountDtos;
using Services.Abstractions;

namespace Services.Implementations
{
    public class CourierAccountService : ICourierAccountService
    {
        private ICourierAccountRepository _courierAccountRepository;

        private IWorkerRoleRepository _workerRoleRepository;

        private IWorkerToRoleRepository _workerToRoleRepository;

        private IRestaurantRepository _restaurantRepository;

        private IMapper _mapper;

        public CourierAccountService(ICourierAccountRepository courierAccountRepository, IWorkerRoleRepository workerRoleRepository, IWorkerToRoleRepository workerToRoleRepository, IRestaurantRepository restaurantRepository, IMapper mapper)
        {
            _courierAccountRepository = courierAccountRepository;
            _workerToRoleRepository = workerToRoleRepository;
            _workerRoleRepository = workerRoleRepository;
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
        }

        public async Task<CreatedDto> CreateCourier(CreateCourierAccountDto createCourierAccountDto)
        {
            var courierAccount = _mapper.Map<CourierAccount>(createCourierAccountDto);

            var findLoginCourierAccount = await _courierAccountRepository.GetByLogin(courierAccount.Login);

            if (findLoginCourierAccount != null)
            {
                throw new("Login already exists");
            }

            var restaurant = await _restaurantRepository.GetById(createCourierAccountDto.RestaurantId);

            if (restaurant == null)
            {
                throw new("Restaurant not found");
            }

            courierAccount.AssignedToRestaurant = restaurant;

            await _courierAccountRepository.Insert(courierAccount);

            return new CreatedDto(courierAccount.Id);
        }

        public async Task AssignToRestaurant(AssignToRestaurantDto assignToRestaurantDto)
        {
            var courierAccount = await _courierAccountRepository.GetById(assignToRestaurantDto.CourierId);

            if (courierAccount == null)
            {
                throw new(MessagesVerbatim.AccountNotFound);
            }

            var restaurant = await _restaurantRepository.GetById(assignToRestaurantDto.RestaurantId);

            if (restaurant == null)
            {
                throw new("Restaurant not found");
            }

            courierAccount.AssignedToRestaurant = restaurant;

            await _courierAccountRepository.Update(courierAccount);
        }

        public async Task<GetRolesResultDto> GetRoles(long courierId)
        {
            var courierAccount = await _courierAccountRepository.GetById(courierId);

            if (courierAccount == null)
            {
                throw new(MessagesVerbatim.AccountNotFound);
            }

            var roles = await _workerToRoleRepository.GetWorkerRoles(courierAccount.Id);

            var roleDtos = _mapper.Map<IEnumerable<WorkerRoleDto>>(roles);

            return new GetRolesResultDto(roleDtos);
        }

        public async Task ChangeCourierData(ChangeAccountDataDto changeAccountDataDto)
        {
            var courierAccount = await _courierAccountRepository.GetById(changeAccountDataDto.CourierId);

            if (courierAccount == null)
            {
                throw new(MessagesVerbatim.AccountNotFound);
            }

            courierAccount.Login = changeAccountDataDto.Login;
            courierAccount.Password = changeAccountDataDto.Password;
            courierAccount.Username = changeAccountDataDto.Username;

            await _courierAccountRepository.Update(courierAccount);
        }
    }
}
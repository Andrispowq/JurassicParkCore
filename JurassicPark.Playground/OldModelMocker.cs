using JurassicPark.Core.OldModel;
using JurassicPark.Core.OldModel.Behaviours;
using JurassicPark.Core.OldModel.Dto;
using JurassicPark.Core.OldModel.Services;
using JurassicPark.Core.OldModel.Validators;

namespace JurassicParkTester;

public class OldModelMocker
{
    public async Task Run()
    {
        IJurassicParkModel model = new JurassicParkModel(
            new AnimalBehaviourHandler(new RandomValueProvider()),
            new RouteValidator());

        var game = await model.CreateGameAsync("hello", Difficulty.Easy, 100, 100);
    }
}
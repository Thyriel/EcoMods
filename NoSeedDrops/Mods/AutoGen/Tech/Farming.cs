namespace Eco.Mods.TechTree
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Eco.Core.Utils;
    using Eco.Core.Utils.AtomicAction;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Property;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Shared.Serialization;
    using Eco.Shared.Services;
    using Eco.Shared.Utils;
    using Gameplay.Systems.Tooltip;

    [Serialized]
    [RequiresSkill(typeof(FarmerSkill), 0)]    
    public partial class FarmingSkill : Skill
    {
        public override string FriendlyName { get { return "Farming"; } }
        public override string Description { get { return ""; } }

        public static int[] SkillPointCost = { 1, 1, 1, 1, 1 };
        public override int RequiredPoint { get { return this.Level < this.MaxLevel ? SkillPointCost[this.Level] : 0; } }
        public override int PrevRequiredPoint { get { return this.Level - 1 >= 0 && this.Level - 1 < this.MaxLevel ? SkillPointCost[this.Level - 1] : 0; } }
        public override int MaxLevel { get { return 1; } }
    }

    [Serialized]
    public partial class FarmingSkillBook : SkillBook<FarmingSkill, FarmingSkillScroll>
    {
        public override string FriendlyName { get { return "Farming Skill Book"; } }
    }

    [Serialized]
    public partial class FarmingSkillScroll : SkillScroll<FarmingSkill, FarmingSkillBook>
    {
        public override string FriendlyName { get { return "Farming Skill Scroll"; } }
    }

    [RequiresSkill(typeof(GatheringSkill), 0)] 
    public partial class FarmingSkillBookRecipe : Recipe
    {
        public FarmingSkillBookRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<FarmingSkillBook>(),
            };
            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<TomatoItem>(typeof(ResearchEfficiencySkill), 10, ResearchEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<CornItem>(typeof(ResearchEfficiencySkill), 10, ResearchEfficiencySkill.MultiplicativeStrategy),
                new CraftingElement<WheatItem>(typeof(ResearchEfficiencySkill), 20, ResearchEfficiencySkill.MultiplicativeStrategy), 
            };
            this.CraftMinutes = new ConstantValue(5);

            this.Initialize("Farming Skill Book", typeof(FarmingSkillBookRecipe));
            CraftingComponent.AddRecipe(typeof(ResearchTableObject), this);
        }
    }
}

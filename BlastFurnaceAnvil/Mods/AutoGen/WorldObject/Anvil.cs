namespace Eco.Mods.TechTree
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Eco.Gameplay.Blocks;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Components.Auth;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Economy;
    using Eco.Gameplay.Housing;
    using Eco.Gameplay.Interactions;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Minimap;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Property;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Gameplay.Pipes.LiquidComponents;
    using Eco.Gameplay.Pipes.Gases;
    using Eco.Gameplay.Systems.Tooltip;
    using Eco.Shared;
    using Eco.Shared.Math;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;
    using Eco.Shared.View;
    using Eco.Shared.Items;
    using Eco.Gameplay.Pipes;
    using Eco.World.Blocks;
    
    [Serialized]    
    [RequireComponent(typeof(AttachmentComponent))]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(MinimapComponent))]                
    [RequireComponent(typeof(LinkComponent))]                   
    [RequireComponent(typeof(CraftingComponent))]               
    [RequireComponent(typeof(HousingComponent))]                          
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireRoomContainment]
    [RequireRoomVolume(25)]                              
    [RequireRoomMaterialTier(2, 18)]        
    public partial class AnvilObject : WorldObject
    {
        public override string FriendlyName { get { return "Anvil"; } } 


        protected override void Initialize()
        {
            this.GetComponent<MinimapComponent>().Initialize("Crafting");                                 
            this.GetComponent<HousingComponent>().Set(AnvilItem.HousingVal);                                



        }

        public override void Destroy()
        {
            base.Destroy();
        }
       
    }

    [Serialized]
    public partial class AnvilItem : WorldObjectItem<AnvilObject>
    {
        public override string FriendlyName { get { return "Anvil"; } } 
        public override string Description { get { return "A solid shaped piece of metal used to hammer ingots into tools and other useful things."; } }

        static AnvilItem()
        {
            
        }
        
        [TooltipChildren] public HousingValue HousingTooltip { get { return HousingVal; } }
        [TooltipChildren] public static HousingValue HousingVal { get { return new HousingValue() 
                                                {
                                                    Category = "Industrial",
                                                    Val = 0,
                                                    TypeForRoomLimit = "",
                                                    DiminishingReturnPercent = 0
                                                };}}       
    }


    [RequiresSkill(typeof(MetalworkingSkill), 1)]
    public partial class AnvilRecipe : Recipe
    {
        public AnvilRecipe()
        {
            this.Products = new CraftingElement[]
            {
                new CraftingElement<AnvilItem>(),
            };

            this.Ingredients = new CraftingElement[]
            {
                new CraftingElement<IronIngotItem>(typeof(MetalworkingEfficiencySkill), 20, MetalworkingEfficiencySkill.MultiplicativeStrategy),   
            };
            SkillModifiedValue value = new SkillModifiedValue(20, MetalworkingSpeedSkill.MultiplicativeStrategy, typeof(MetalworkingSpeedSkill), "craft time");
            SkillModifiedValueManager.AddBenefitForObject(typeof(AnvilRecipe), Item.Get<AnvilItem>().UILink(), value);
            SkillModifiedValueManager.AddSkillBenefit(Item.Get<AnvilItem>().UILink(), value);
            this.CraftMinutes = value;
            this.Initialize("Anvil", typeof(AnvilRecipe));
            CraftingComponent.AddRecipe(typeof(BloomeryObject), this);
		 CraftingComponent.AddRecipe(typeof(BlastFurnaceObject), this);
        }
    }
}

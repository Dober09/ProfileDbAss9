
using Google.Android.Material.Badge;
using Google.Android.Material.BottomNavigation;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Platform;
using ProfileAss.Service;

namespace ProfileAss
{
    public class TabbarBadgeRenderer : ShellRenderer
    {

        protected override IShellBottomNavViewAppearanceTracker CreateBottomNavViewAppearanceTracker(ShellItem shellItem)
        {
            //return base.CreateBottomNavViewAppearanceTracker(shellItem);
            return new BadgeShellBottomNavViewAppearanceTracker(this, shellItem);
        }

    }

    class BadgeShellBottomNavViewAppearanceTracker : ShellBottomNavViewAppearanceTracker
    {
        private BadgeDrawable? basketBadgeDrawable;
     
        public BadgeShellBottomNavViewAppearanceTracker(IShellContext shellContext, ShellItem shellItem) : base(shellContext, shellItem)
        {
        }

        public override void SetAppearance(BottomNavigationView bottomView, IShellAppearanceElement appearance)
        {
            base.SetAppearance(bottomView, appearance);
            // Basket Badge
            if (basketBadgeDrawable is null)
            {
                const int basketIdx = 3;
                basketBadgeDrawable = bottomView.GetOrCreateBadge(basketIdx);
                UpdateBasketBadge(BadgeCounterService.Count);
                BadgeCounterService.CountChanged += OnBasketCountChanged;
                
            }

        }

        private void OnBasketCountChanged(object? sender, int newCount)
        {
            UpdateBasketBadge(newCount);
        }


        private void UpdateBasketBadge(int count)
        {
            if (basketBadgeDrawable is not null)
            {
                if (count <= 0)
                {
                    basketBadgeDrawable.SetVisible(false);
                }
                else
                {
                    basketBadgeDrawable.Number = count;
                    basketBadgeDrawable.BackgroundColor = Colors.Red.ToPlatform();
                    basketBadgeDrawable.BadgeTextColor = Colors.White.ToPlatform();
                    basketBadgeDrawable.SetVisible(true);
                }
            }
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            BadgeCounterService.CountChanged -= OnBasketCountChanged;
        }





    }


}


using GatherBuddy.Classes;

namespace GatherBuddy.Managers
{
    public static class FishData
    {
        public static void Apply(FishManager fish)
        {
        Fish f;
            if( fish.Fish.TryGetValue(4898, out f) )
                f.CatchData = new CatchData(2, 00, 18, 6){ BaitOrder = new(){ 2596 } };
            if( fish.Fish.TryGetValue(4904, out f) )
                f.CatchData = new CatchData(2, 00){ BaitOrder = new(){ 2585, 4869 } };
            if( fish.Fish.TryGetValue(4905, out f) )
                f.CatchData = new CatchData(2, 00){ CurrentWeather = new(){ 7, 15 }, BaitOrder = new(){ 2601 } };
            if( fish.Fish.TryGetValue(4906, out f) )
                f.CatchData = new CatchData(2, 00){ BaitOrder = new(){ 2616, 4898 } };
            if( fish.Fish.TryGetValue(4911, out f) )
                f.CatchData = new CatchData(2, 00, 17, 21){ BaitOrder = new(){ 2606 } };
            if( fish.Fish.TryGetValue(4913, out f) )
                f.CatchData = new CatchData(2, 00){ CurrentWeather = new(){ 7, 8 }, BaitOrder = new(){ 2606 } };
            if( fish.Fish.TryGetValue(4915, out f) )
                f.CatchData = new CatchData(2, 00, 10, 18){ BaitOrder = new(){ 2628 } };
            if( fish.Fish.TryGetValue(4918, out f) )
                f.CatchData = new CatchData(2, 00, 18, 5){ BaitOrder = new(){ 2616, 4898 } };
            if( fish.Fish.TryGetValue(4924, out f) )
                f.CatchData = new CatchData(2, 00, 9, 15){ CurrentWeather = new(){ 1, 2 }, BaitOrder = new(){ 2585, 4869, 4904 } };
            if( fish.Fish.TryGetValue(4991, out f) )
                f.CatchData = new CatchData(2, 00){ CurrentWeather = new(){ 7, 8 }, BaitOrder = new(){ 2620 } };
            if( fish.Fish.TryGetValue(5007, out f) )
                f.CatchData = new CatchData(2, 00){ CurrentWeather = new(){ 14, 2, 1 }, BaitOrder = new(){ 2603 } };
            if( fish.Fish.TryGetValue(5008, out f) )
                f.CatchData = new CatchData(2, 00){ BaitOrder = new(){ 2603 } };
            if( fish.Fish.TryGetValue(5016, out f) )
                f.CatchData = new CatchData(2, 00, 4, 9){ BaitOrder = new(){ 2626 } };
            if( fish.Fish.TryGetValue(5017, out f) )
                f.CatchData = new CatchData(2, 00, 10, 15){ CurrentWeather = new(){ 1, 2 }, BaitOrder = new(){ 2607 } };
            if( fish.Fish.TryGetValue(5021, out f) )
                f.CatchData = new CatchData(2, 00){ CurrentWeather = new(){ 17 }, BaitOrder = new(){ 2625 } };
            if( fish.Fish.TryGetValue(5022, out f) )
                f.CatchData = new CatchData(2, 00){ CurrentWeather = new(){ 4, 3 }, BaitOrder = new(){ 2620, 4995 } };
            if( fish.Fish.TryGetValue(5023, out f) )
                f.CatchData = new CatchData(2, 00){ CurrentWeather = new(){ 9, 10 }, BaitOrder = new(){ 2607 } };
            if( fish.Fish.TryGetValue(5027, out f) )
                f.CatchData = new CatchData(2, 00, 17, 9){ BaitOrder = new(){ 2607 } };
            if( fish.Fish.TryGetValue(5031, out f) )
                f.CatchData = new CatchData(2, 00){ BaitOrder = new(){ 2599, 4978, 5011 } };
            if( fish.Fish.TryGetValue(5544, out f) )
                f.CatchData = new CatchData(2, 00){ BaitOrder = new(){ 2610 } };
            if( fish.Fish.TryGetValue(4869, out f) )
                f.CatchData = new CatchData(2, 00){ Snagging = false, BaitOrder = new(){ 2585 } };
            if( fish.Fish.TryGetValue(5011, out f) )
                f.CatchData = new CatchData(2, 00){ Snagging = false, BaitOrder = new(){ 2599, 4978 } };
            if( fish.Fish.TryGetValue(4978, out f) )
                f.CatchData = new CatchData(2, 00){ Snagging = false, BaitOrder = new(){ 2599 } };
            if( fish.Fish.TryGetValue(4942, out f) )
                f.CatchData = new CatchData(2, 00){ Snagging = false, BaitOrder = new(){ 2592 } };
            if( fish.Fish.TryGetValue(4927, out f) )
                f.CatchData = new CatchData(2, 00){ Snagging = false, BaitOrder = new(){ 2586 } };
            if( fish.Fish.TryGetValue(4874, out f) )
                f.CatchData = new CatchData(2, 00){ Snagging = false, BaitOrder = new(){ 2587 } };
            if( fish.Fish.TryGetValue(4948, out f) )
                f.CatchData = new CatchData(2, 00){ Snagging = false, BaitOrder = new(){ 2594 } };
            if( fish.Fish.TryGetValue(5035, out f) )
                f.CatchData = new CatchData(2, 00){ Snagging = false, BaitOrder = new(){ 2600 } };
            if( fish.Fish.TryGetValue(4995, out f) )
                f.CatchData = new CatchData(2, 00){ Snagging = false, BaitOrder = new(){ 2626 } };
            if( fish.Fish.TryGetValue(4872, out f) )
                f.CatchData = new CatchData(2, 00){ Snagging = false, BaitOrder = new(){ 2587 } };
            if( fish.Fish.TryGetValue(4937, out f) )
                f.CatchData = new CatchData(2, 00){ Snagging = false, BaitOrder = new(){ 2599 } };
            if( fish.Fish.TryGetValue(5040, out f) )
                f.CatchData = new CatchData(2, 00){ Snagging = false, BaitOrder = new(){ 2605 } };
            if( fish.Fish.TryGetValue(4919, out f) )
                f.CatchData = new CatchData(2, 00){ Snagging = false, BaitOrder = new(){ 2585, 4869, 4904 } };
            if( fish.Fish.TryGetValue(4888, out f) )
                f.CatchData = new CatchData(2, 00){ Snagging = false, BaitOrder = new(){ 2587, 4874 } };
            if( fish.Fish.TryGetValue(5002, out f) )
                f.CatchData = new CatchData(2, 00){ Snagging = false, BaitOrder = new(){ 2599, 4978 } };
            if( fish.Fish.TryGetValue(7678, out f) )
                f.CatchData = new CatchData(2, 20, 9, 14){ BaitOrder = new(){ 2591 } };
            if( fish.Fish.TryGetValue(7679, out f) )
                f.CatchData = new CatchData(2, 20, 9, 14){ BaitOrder = new(){ 2586 } };
            if( fish.Fish.TryGetValue(7680, out f) )
                f.CatchData = new CatchData(2, 20){ BaitOrder = new(){ 2589 } };
            if( fish.Fish.TryGetValue(7681, out f) )
                f.CatchData = new CatchData(2, 20){ BaitOrder = new(){ 2586 } };
            if( fish.Fish.TryGetValue(7682, out f) )
                f.CatchData = new CatchData(2, 20){ CurrentWeather = new(){ 4, 3 }, BaitOrder = new(){ 2594 } };
            if( fish.Fish.TryGetValue(7683, out f) )
                f.CatchData = new CatchData(2, 20, 18, 2){ BaitOrder = new(){ 2592, 4942 } };
            if( fish.Fish.TryGetValue(7684, out f) )
                f.CatchData = new CatchData(2, 20, 16, 2){ BaitOrder = new(){ 2585, 4869, 4904 } };
            if( fish.Fish.TryGetValue(7685, out f) )
                f.CatchData = new CatchData(2, 20, 9, 14){ BaitOrder = new(){ 2587 } };
            if( fish.Fish.TryGetValue(7686, out f) )
                f.CatchData = new CatchData(2, 20){ CurrentWeather = new(){ 1, 2 }, BaitOrder = new(){ 2585, 4869 } };
            if( fish.Fish.TryGetValue(7687, out f) )
                f.CatchData = new CatchData(2, 20, 19, 3){ BaitOrder = new(){ 2606 } };
            if( fish.Fish.TryGetValue(7688, out f) )
                f.CatchData = new CatchData(2, 20){ BaitOrder = new(){ 2588 } };
            if( fish.Fish.TryGetValue(7689, out f) )
                f.CatchData = new CatchData(2, 20){ CurrentWeather = new(){ 4, 3, 11 }, BaitOrder = new(){ 2614 } };
            if( fish.Fish.TryGetValue(7690, out f) )
                f.CatchData = new CatchData(2, 20){ BaitOrder = new(){ 2628 } };
            if( fish.Fish.TryGetValue(7691, out f) )
                f.CatchData = new CatchData(2, 20, 16, 22){ BaitOrder = new(){ 2589 } };
            if( fish.Fish.TryGetValue(7692, out f) )
                f.CatchData = new CatchData(2, 20, 17, 3){ BaitOrder = new(){ 2588 } };
            if( fish.Fish.TryGetValue(7693, out f) )
                f.CatchData = new CatchData(2, 20, 9, 14){ CurrentWeather = new(){ 1, 2 }, BaitOrder = new(){ 2628 } };
            if( fish.Fish.TryGetValue(7694, out f) )
                f.CatchData = new CatchData(2, 20, 21, 3){ BaitOrder = new(){ 2586, 4927 } };
            if( fish.Fish.TryGetValue(7695, out f) )
                f.CatchData = new CatchData(2, 20, 17, 21){ BaitOrder = new(){ 2623 } };
            if( fish.Fish.TryGetValue(7696, out f) )
                f.CatchData = new CatchData(2, 20){ BaitOrder = new(){ 2588 } };
            if( fish.Fish.TryGetValue(7697, out f) )
                f.CatchData = new CatchData(2, 20){ CurrentWeather = new(){ 4, 3, 5 }, BaitOrder = new(){ 2626 } };
            if( fish.Fish.TryGetValue(7698, out f) )
                f.CatchData = new CatchData(2, 20, 19){ BaitOrder = new(){ 2588 } };
            if( fish.Fish.TryGetValue(7699, out f) )
                f.CatchData = new CatchData(2, 20, 9, 14){ BaitOrder = new(){ 2611 } };
            if( fish.Fish.TryGetValue(7700, out f) )
                f.CatchData = new CatchData(2, 20, 21, 3){ CurrentWeather = new(){ 7 }, BaitOrder = new(){ 2594 } };
            if( fish.Fish.TryGetValue(7701, out f) )
                f.CatchData = new CatchData(2, 20, 9, 14){ CurrentWeather = new(){ 4, 3 }, BaitOrder = new(){ 2614 } };
            if( fish.Fish.TryGetValue(7702, out f) )
                f.CatchData = new CatchData(2, 20, 17, 8){ CurrentWeather = new(){ 7, 8 }, BaitOrder = new(){ 2592 } };
            if( fish.Fish.TryGetValue(7703, out f) )
                f.CatchData = new CatchData(2, 20, 17, 21){ BaitOrder = new(){ 2590 } };
            if( fish.Fish.TryGetValue(7704, out f) )
                f.CatchData = new CatchData(2, 20, 20, 3){ CurrentWeather = new(){ 7, 8 }, BaitOrder = new(){ 2597 } };
            if( fish.Fish.TryGetValue(7705, out f) )
                f.CatchData = new CatchData(2, 20){ BaitOrder = new(){ 2586, 4927 } };
            if( fish.Fish.TryGetValue(7706, out f) )
                f.CatchData = new CatchData(2, 20, 21, 3){ BaitOrder = new(){ 2594 } };
            if( fish.Fish.TryGetValue(7707, out f) )
                f.CatchData = new CatchData(2, 20, 9, 17){ BaitOrder = new(){ 2587, 4874 } };
            if( fish.Fish.TryGetValue(7708, out f) )
                f.CatchData = new CatchData(2, 20, 15, 21){ BaitOrder = new(){ 2590 } };
            if( fish.Fish.TryGetValue(7709, out f) )
                f.CatchData = new CatchData(2, 20, 5, 8){ BaitOrder = new(){ 2617 } };
            if( fish.Fish.TryGetValue(7710, out f) )
                f.CatchData = new CatchData(2, 20){ CurrentWeather = new(){ 7 }, BaitOrder = new(){ 2596 } };
            if( fish.Fish.TryGetValue(7711, out f) )
                f.CatchData = new CatchData(2, 20){ BaitOrder = new(){ 2619 } };
            if( fish.Fish.TryGetValue(7712, out f) )
                f.CatchData = new CatchData(2, 20, 21, 3){ BaitOrder = new(){ 2589 } };
            if( fish.Fish.TryGetValue(7713, out f) )
                f.CatchData = new CatchData(2, 20, 17, 2){ CurrentWeather = new(){ 7, 15 }, BaitOrder = new(){ 2601 } };
            if( fish.Fish.TryGetValue(7714, out f) )
                f.CatchData = new CatchData(2, 20){ CurrentWeather = new(){ 1, 2 }, BaitOrder = new(){ 2594 } };
            if( fish.Fish.TryGetValue(7902, out f) )
                f.CatchData = new CatchData(2, 30, 17, 19){ BaitOrder = new(){ 2597 } };
            if( fish.Fish.TryGetValue(7903, out f) )
                f.CatchData = new CatchData(2, 30, 3, 5){ CurrentWeather = new(){ 3, 5 }, BaitOrder = new(){ 2591 } };
            if( fish.Fish.TryGetValue(7904, out f) )
                f.CatchData = new CatchData(2, 30, 4, 6){ BaitOrder = new(){ 2614 } };
            if( fish.Fish.TryGetValue(7905, out f) )
                f.CatchData = new CatchData(2, 30, 17, 20){ CurrentWeather = new(){ 4, 3 }, BaitOrder = new(){ 2620, 4995 } };
            if( fish.Fish.TryGetValue(7906, out f) )
                f.CatchData = new CatchData(2, 30, 17, 18){ BaitOrder = new(){ 2606 } };
            if( fish.Fish.TryGetValue(7907, out f) )
                f.CatchData = new CatchData(2, 30, 21, 23){ BaitOrder = new(){ 2594 } };
            if( fish.Fish.TryGetValue(7908, out f) )
                f.CatchData = new CatchData(2, 30, 18, 19){ BaitOrder = new(){ 2594 } };
            if( fish.Fish.TryGetValue(7909, out f) )
                f.CatchData = new CatchData(2, 30, 20, 23){ BaitOrder = new(){ 2613 } };
            if( fish.Fish.TryGetValue(7910, out f) )
                f.CatchData = new CatchData(2, 30, 20, 22){ BaitOrder = new(){ 2606 } };
            if( fish.Fish.TryGetValue(7911, out f) )
                f.CatchData = new CatchData(2, 30){ CurrentWeather = new(){ 4 }, BaitOrder = new(){ 2592, 4948 } };
            if( fish.Fish.TryGetValue(7912, out f) )
                f.CatchData = new CatchData(2, 30){ CurrentWeather = new(){ 7 }, BaitOrder = new(){ 2594, 4948 } };
            if( fish.Fish.TryGetValue(7913, out f) )
                f.CatchData = new CatchData(2, 30, 0, 8){ CurrentWeather = new(){ 4, 3 }, BaitOrder = new(){ 2595 } };
            if( fish.Fish.TryGetValue(7914, out f) )
                f.CatchData = new CatchData(2, 30){ BaitOrder = new(){ 2597 } };
            if( fish.Fish.TryGetValue(7915, out f) )
                f.CatchData = new CatchData(2, 30, 19, 21){ CurrentWeather = new(){ 1, 2 }, BaitOrder = new(){ 2596, 4898 } };
            if( fish.Fish.TryGetValue(7916, out f) )
                f.CatchData = new CatchData(2, 30, 19, 22){ BaitOrder = new(){ 2594 } };
            if( fish.Fish.TryGetValue(7917, out f) )
                f.CatchData = new CatchData(2, 30, 4, 6){ CurrentWeather = new(){ 1, 2 }, BaitOrder = new(){ 2596, 4898 } };
            if( fish.Fish.TryGetValue(7918, out f) )
                f.CatchData = new CatchData(2, 30, 17, 20){ CurrentWeather = new(){ 1 }, BaitOrder = new(){ 2599 } };
            if( fish.Fish.TryGetValue(7919, out f) )
                f.CatchData = new CatchData(2, 30, 21){ CurrentWeather = new(){ 4, 3 }, BaitOrder = new(){ 2607 } };
            if( fish.Fish.TryGetValue(7920, out f) )
                f.CatchData = new CatchData(2, 30, 18, 23){ BaitOrder = new(){ 2597 } };
            if( fish.Fish.TryGetValue(7921, out f) )
                f.CatchData = new CatchData(2, 30, 21, 3){ CurrentWeather = new(){ 1, 2 }, BaitOrder = new(){ 2617 } };
            if( fish.Fish.TryGetValue(7922, out f) )
                f.CatchData = new CatchData(2, 30){ CurrentWeather = new(){ 9, 10 }, BaitOrder = new(){ 2599, 4978 } };
            if( fish.Fish.TryGetValue(7923, out f) )
                f.CatchData = new CatchData(2, 30, 23, 5){ CurrentWeather = new(){ 4 }, BaitOrder = new(){ 2592, 4942 } };
            if( fish.Fish.TryGetValue(7924, out f) )
                f.CatchData = new CatchData(2, 30, 9, 15){ CurrentWeather = new(){ 1 }, BaitOrder = new(){ 2598 } };
            if( fish.Fish.TryGetValue(7925, out f) )
                f.CatchData = new CatchData(2, 30, 17, 20){ BaitOrder = new(){ 2599 } };
            if( fish.Fish.TryGetValue(7926, out f) )
                f.CatchData = new CatchData(2, 30){ CurrentWeather = new(){ 6 }, BaitOrder = new(){ 2619 } };
            if( fish.Fish.TryGetValue(7927, out f) )
                f.CatchData = new CatchData(2, 30, 16, 19){ BaitOrder = new(){ 2626 } };
            if( fish.Fish.TryGetValue(7928, out f) )
                f.CatchData = new CatchData(2, 30, 12, 18){ CurrentWeather = new(){ 14 }, BaitOrder = new(){ 2603 } };
            if( fish.Fish.TryGetValue(7929, out f) )
                f.CatchData = new CatchData(2, 30){ CurrentWeather = new(){ 4 }, BaitOrder = new(){ 2620 } };
            if( fish.Fish.TryGetValue(7930, out f) )
                f.CatchData = new CatchData(2, 30){ CurrentWeather = new(){ 14 }, BaitOrder = new(){ 2600, 5035 } };
            if( fish.Fish.TryGetValue(7931, out f) )
                f.CatchData = new CatchData(2, 30, 9, 16){ CurrentWeather = new(){ 14 }, BaitOrder = new(){ 2600, 5035 } };
            if( fish.Fish.TryGetValue(7932, out f) )
                f.CatchData = new CatchData(2, 30){ CurrentWeather = new(){ 16 }, BaitOrder = new(){ 2623 } };
            if( fish.Fish.TryGetValue(7933, out f) )
                f.CatchData = new CatchData(2, 30, 0, 4){ BaitOrder = new(){ 2603 } };
            if( fish.Fish.TryGetValue(7934, out f) )
                f.CatchData = new CatchData(2, 30, 18, 21){ BaitOrder = new(){ 2599 } };
            if( fish.Fish.TryGetValue(7935, out f) )
                f.CatchData = new CatchData(2, 30){ CurrentWeather = new(){ 9 }, BaitOrder = new(){ 2599, 4978 } };
            if( fish.Fish.TryGetValue(7936, out f) )
                f.CatchData = new CatchData(2, 30){ CurrentWeather = new(){ 10 }, BaitOrder = new(){ 2607 } };
            if( fish.Fish.TryGetValue(7937, out f) )
                f.CatchData = new CatchData(2, 30, 19, 3){ CurrentWeather = new(){ 7 }, BaitOrder = new(){ 2601 } };
            if( fish.Fish.TryGetValue(7938, out f) )
                f.CatchData = new CatchData(2, 30){ CurrentWeather = new(){ 1, 2 }, BaitOrder = new(){ 2618 } };
            if( fish.Fish.TryGetValue(7939, out f) )
                f.CatchData = new CatchData(2, 30){ CurrentWeather = new(){ 4 }, BaitOrder = new(){ 2599 } };
            if( fish.Fish.TryGetValue(7940, out f) )
                f.CatchData = new CatchData(2, 30, 9, 16){ CurrentWeather = new(){ 1, 2 }, BaitOrder = new(){ 2592, 4948 } };
            if( fish.Fish.TryGetValue(7941, out f) )
                f.CatchData = new CatchData(2, 30, 9, 15){ CurrentWeather = new(){ 4, 3, 5 }, BaitOrder = new(){ 2587, 4872 } };
            if( fish.Fish.TryGetValue(7942, out f) )
                f.CatchData = new CatchData(2, 30){ CurrentWeather = new(){ 4 }, BaitOrder = new(){ 2601 } };
            if( fish.Fish.TryGetValue(7943, out f) )
                f.CatchData = new CatchData(2, 30, 22, 4){ CurrentWeather = new(){ 4 }, BaitOrder = new(){ 2626, 4995 } };
            if( fish.Fish.TryGetValue(7944, out f) )
                f.CatchData = new CatchData(2, 30, 5, 7){ CurrentWeather = new(){ 1, 2 }, BaitOrder = new(){ 2623 } };
            if( fish.Fish.TryGetValue(7945, out f) )
                f.CatchData = new CatchData(2, 30, 21, 4){ CurrentWeather = new(){ 1, 2 }, BaitOrder = new(){ 2597, 4937 } };
            if( fish.Fish.TryGetValue(7946, out f) )
                f.CatchData = new CatchData(2, 30){ BaitOrder = new(){ 2599, 4937 } };
            if( fish.Fish.TryGetValue(7947, out f) )
                f.CatchData = new CatchData(2, 30, 4, 10){ CurrentWeather = new(){ 4 }, BaitOrder = new(){ 2624 } };
            if( fish.Fish.TryGetValue(7948, out f) )
                f.CatchData = new CatchData(2, 30, 23, 2){ BaitOrder = new(){ 2616, 4898 } };
            if( fish.Fish.TryGetValue(7949, out f) )
                f.CatchData = new CatchData(2, 30, 10, 15){ CurrentWeather = new(){ 1, 2 }, BaitOrder = new(){ 2605, 5040 } };
            if( fish.Fish.TryGetValue(7950, out f) )
                f.CatchData = new CatchData(2, 30, 17, 3){ CurrentWeather = new(){ 4, 3, 5 }, BaitOrder = new(){ 2585, 4869, 4904, 4919 } };
            if( fish.Fish.TryGetValue(7951, out f) )
                f.CatchData = new CatchData(2, 30, 3, 13){ CurrentWeather = new(){ 17 }, BaitOrder = new(){ 2603 } };
            if( fish.Fish.TryGetValue(8752, out f) )
                f.CatchData = new CatchData(2, 40){ Predator = (5031, 3), BaitOrder = new(){ 2599, 4978 } };
            if( fish.Fish.TryGetValue(8753, out f) )
                f.CatchData = new CatchData(2, 40){ PreviousWeather = new(){ 7, 8 }, CurrentWeather = new(){ 1 }, BaitOrder = new(){ 2587, 4874, 4888 } };
            if( fish.Fish.TryGetValue(8754, out f) )
                f.CatchData = new CatchData(2, 40){ Predator = (4913, 3), CurrentWeather = new(){ 7, 8 }, BaitOrder = new(){ 2606 } };
            if( fish.Fish.TryGetValue(8755, out f) )
                f.CatchData = new CatchData(2, 40, 22, 3){ CurrentWeather = new(){ 4, 3, 5 }, BaitOrder = new(){ 2596, 4898 } };
            if( fish.Fish.TryGetValue(8756, out f) )
                f.CatchData = new CatchData(2, 40, 20, 5){ PreviousWeather = new(){ 1, 2 }, CurrentWeather = new(){ 4, 3, 5 }, BaitOrder = new(){ 2596, 4898 } };
            if( fish.Fish.TryGetValue(8757, out f) )
                f.CatchData = new CatchData(2, 40, 19, 2){ PreviousWeather = new(){ 1, 2 }, CurrentWeather = new(){ 1, 2 }, BaitOrder = new(){ 2585, 4869, 4904 } };
            if( fish.Fish.TryGetValue(8758, out f) )
                f.CatchData = new CatchData(2, 40){ PreviousWeather = new(){ 17 }, CurrentWeather = new(){ 1, 2 }, BaitOrder = new(){ 2603 } };
            if( fish.Fish.TryGetValue(8759, out f) )
                f.CatchData = new CatchData(2, 40){ PreviousWeather = new(){ 1, 2 }, CurrentWeather = new(){ 17 }, BaitOrder = new(){ 2603 } };
            if( fish.Fish.TryGetValue(8760, out f) )
                f.CatchData = new CatchData(2, 40){ Predator = (5008, 5), BaitOrder = new(){ 2625 } };
            if( fish.Fish.TryGetValue(8761, out f) )
                f.CatchData = new CatchData(2, 40, 18, 9){ CurrentWeather = new(){ 17 }, BaitOrder = new(){ 2592, 4942, 5002 } };
            if( fish.Fish.TryGetValue(8762, out f) )
                f.CatchData = new CatchData(2, 40, 8, 18){ CurrentWeather = new(){ 17 }, BaitOrder = new(){ 2599, 4978, 5011 } };
            if( fish.Fish.TryGetValue(8763, out f) )
                f.CatchData = new CatchData(2, 40){ Predator = (8762, 1), BaitOrder = new(){ 2599, 4978, 5002 } };
            if( fish.Fish.TryGetValue(8764, out f) )
                f.CatchData = new CatchData(2, 40){ Predator = (4904, 6), BaitOrder = new(){ 2585, 4869, 4904 } };
            if( fish.Fish.TryGetValue(8765, out f) )
                f.CatchData = new CatchData(2, 40, 18, 5){ CurrentWeather = new(){ 4 }, BaitOrder = new(){ 2599, 4978, 5002 } };
            if( fish.Fish.TryGetValue(8766, out f) )
                f.CatchData = new CatchData(2, 40){ PreviousWeather = new(){ 7 }, CurrentWeather = new(){ 4, 3, 1, 2 }, BaitOrder = new(){ 2599, 4978 } };
            if( fish.Fish.TryGetValue(8767, out f) )
                f.CatchData = new CatchData(2, 40, 1, 4){ PreviousWeather = new(){ 1, 2 }, CurrentWeather = new(){ 4 }, BaitOrder = new(){ 2599, 4978 } };
            if( fish.Fish.TryGetValue(8768, out f) )
                f.CatchData = new CatchData(2, 40, 8, 20){ PreviousWeather = new(){ 4, 3 }, CurrentWeather = new(){ 14 }, BaitOrder = new(){ 2600, 5035 } };
            if( fish.Fish.TryGetValue(8769, out f) )
                f.CatchData = new CatchData(2, 40, 19, 4){ Predator = (5544, 5), CurrentWeather = new(){ 16, 15 }, BaitOrder = new(){ 2599, 4937 } };
            if( fish.Fish.TryGetValue(8770, out f) )
                f.CatchData = new CatchData(2, 40){ PreviousWeather = new(){ 16, 15 }, CurrentWeather = new(){ 1, 2 }, BaitOrder = new(){ 2607 } };
            if( fish.Fish.TryGetValue(8771, out f) )
                f.CatchData = new CatchData(2, 40){ PreviousWeather = new(){ 1, 2 }, CurrentWeather = new(){ 16 }, BaitOrder = new(){ 2605, 5040 } };
            if( fish.Fish.TryGetValue(8772, out f) )
                f.CatchData = new CatchData(2, 40){ PreviousWeather = new(){ 1, 2 }, CurrentWeather = new(){ 16 }, BaitOrder = new(){ 2605, 5040, 8771 } };
            if( fish.Fish.TryGetValue(8773, out f) )
                f.CatchData = new CatchData(2, 40){ PreviousWeather = new(){ 7, 9 }, CurrentWeather = new(){ 4, 3 }, BaitOrder = new(){ 2620, 4995 } };
            if( fish.Fish.TryGetValue(8774, out f) )
                f.CatchData = new CatchData(2, 40){ CurrentWeather = new(){ 9, 10 }, BaitOrder = new(){ 2603 } };
            if( fish.Fish.TryGetValue(8775, out f) )
                f.CatchData = new CatchData(2, 40){ Predator = (8774, 1), BaitOrder = new(){ 2624 } };
            if( fish.Fish.TryGetValue(8776, out f) )
                f.CatchData = new CatchData(2, 40, 4, 12){ PreviousWeather = new(){ 1, 2 }, CurrentWeather = new(){ 4 }, BaitOrder = new(){ 2599, 4978 } };
            if( fish.Fish.TryGetValue(10123, out f) )
                f.CatchData = new CatchData(2, 50, 23, 2){ Snagging = false, BaitOrder = new(){ 2619 } };
            if( fish.Fish.TryGetValue(12713, out f) )
                f.CatchData = new CatchData(3, 00){ BaitOrder = new(){ 12706 } };
            if( fish.Fish.TryGetValue(12715, out f) )
                f.CatchData = new CatchData(3, 00){ BaitOrder = new(){ 12705 } };
            if( fish.Fish.TryGetValue(12721, out f) )
                f.CatchData = new CatchData(3, 00){ CurrentWeather = new(){ 3, 4, 11 }, BaitOrder = new(){ 12707 } };
            if( fish.Fish.TryGetValue(12723, out f) )
                f.CatchData = new CatchData(3, 00){ CurrentWeather = new(){ 2, 1 }, BaitOrder = new(){ 12708 } };
            if( fish.Fish.TryGetValue(12724, out f) )
                f.CatchData = new CatchData(3, 00){ CurrentWeather = new(){ 16, 15 }, BaitOrder = new(){ 12708 } };
            if( fish.Fish.TryGetValue(12726, out f) )
                f.CatchData = new CatchData(3, 00, 8, 20){ BaitOrder = new(){ 2599, 4937 } };
            if( fish.Fish.TryGetValue(12727, out f) )
                f.CatchData = new CatchData(3, 00){ CurrentWeather = new(){ 15, 16 }, BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(12733, out f) )
                f.CatchData = new CatchData(3, 00){ Snagging = true, BaitOrder = new(){ 12706 } };
            if( fish.Fish.TryGetValue(12739, out f) )
                f.CatchData = new CatchData(3, 00, 10, 18){ BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(12740, out f) )
                f.CatchData = new CatchData(3, 00){ CurrentWeather = new(){ 11, 3, 4 }, BaitOrder = new(){ 12707 } };
            if( fish.Fish.TryGetValue(12741, out f) )
                f.CatchData = new CatchData(3, 00){ CurrentWeather = new(){ 11, 4, 3 }, BaitOrder = new(){ 12706 } };
            if( fish.Fish.TryGetValue(12742, out f) )
                f.CatchData = new CatchData(3, 00, 16, 19){ BaitOrder = new(){ 12704 } };
            if( fish.Fish.TryGetValue(12743, out f) )
                f.CatchData = new CatchData(3, 00, 9, 2){ BaitOrder = new(){ 12704 } };
            if( fish.Fish.TryGetValue(12746, out f) )
                f.CatchData = new CatchData(3, 00){ CurrentWeather = new(){ 3, 4, 5 }, BaitOrder = new(){ 12708 } };
            if( fish.Fish.TryGetValue(12749, out f) )
                f.CatchData = new CatchData(3, 00){ CurrentWeather = new(){ 1, 2 }, BaitOrder = new(){ 12707 } };
            if( fish.Fish.TryGetValue(12751, out f) )
                f.CatchData = new CatchData(3, 00){ CurrentWeather = new(){ 3 }, BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(12753, out f) )
                f.CatchData = new CatchData(3, 00){ BaitOrder = new(){ 12708 } };
            if( fish.Fish.TryGetValue(12757, out f) )
                f.CatchData = new CatchData(3, 00){ BaitOrder = new(){ 12704 } };
            if( fish.Fish.TryGetValue(12761, out f) )
                f.CatchData = new CatchData(3, 00, 0, 6){ CurrentWeather = new(){ 11, 3, 4 }, BaitOrder = new(){ 12704, 12722 } };
            if( fish.Fish.TryGetValue(12762, out f) )
                f.CatchData = new CatchData(3, 00){ CurrentWeather = new(){ 15, 16 }, BaitOrder = new(){ 12705 } };
            if( fish.Fish.TryGetValue(12763, out f) )
                f.CatchData = new CatchData(3, 00){ CurrentWeather = new(){ 15, 16 }, BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(12765, out f) )
                f.CatchData = new CatchData(3, 00){ CurrentWeather = new(){ 15, 16 }, BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(12766, out f) )
                f.CatchData = new CatchData(3, 00){ CurrentWeather = new(){ 15, 16 }, BaitOrder = new(){ 12708, 12724 } };
            if( fish.Fish.TryGetValue(12767, out f) )
                f.CatchData = new CatchData(3, 00, 21, 3){ BaitOrder = new(){ 12705 } };
            if( fish.Fish.TryGetValue(12768, out f) )
                f.CatchData = new CatchData(3, 00){ BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(12774, out f) )
                f.CatchData = new CatchData(3, 00, 21){ BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(12784, out f) )
                f.CatchData = new CatchData(3, 00, 10, 14){ CurrentWeather = new(){ 1, 2 }, BaitOrder = new(){ 12708, 12753, 12805 } };
            if( fish.Fish.TryGetValue(12786, out f) )
                f.CatchData = new CatchData(3, 00, 18, 5){ BaitOrder = new(){ 12709, 12754 } };
            if( fish.Fish.TryGetValue(12789, out f) )
                f.CatchData = new CatchData(3, 00){ CurrentWeather = new(){ 3 }, BaitOrder = new(){ 12708 } };
            if( fish.Fish.TryGetValue(12792, out f) )
                f.CatchData = new CatchData(3, 00, 8, 12){ BaitOrder = new(){ 12706 } };
            if( fish.Fish.TryGetValue(12796, out f) )
                f.CatchData = new CatchData(3, 00, 8, 17){ BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(12799, out f) )
                f.CatchData = new CatchData(3, 00){ CurrentWeather = new(){ 2, 1 }, BaitOrder = new(){ 12712, 12716 } };
            if( fish.Fish.TryGetValue(12800, out f) )
                f.CatchData = new CatchData(3, 00){ Snagging = false, BaitOrder = new(){ 12709 } };
            if( fish.Fish.TryGetValue(12802, out f) )
                f.CatchData = new CatchData(3, 00, 18, 21){ BaitOrder = new(){ 12707, 12730 } };
            if( fish.Fish.TryGetValue(12803, out f) )
                f.CatchData = new CatchData(3, 00, 18, 2){ CurrentWeather = new(){ 3 }, BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(12804, out f) )
                f.CatchData = new CatchData(3, 00){ BaitOrder = new(){ 12704, 12757 } };
            if( fish.Fish.TryGetValue(12805, out f) )
                f.CatchData = new CatchData(3, 00){ BaitOrder = new(){ 12712 } };
            if( fish.Fish.TryGetValue(12810, out f) )
                f.CatchData = new CatchData(3, 00){ BaitOrder = new(){ 12712 } };
            if( fish.Fish.TryGetValue(12812, out f) )
                f.CatchData = new CatchData(3, 00){ CurrentWeather = new(){ 9 }, BaitOrder = new(){ 12710 } };
            if( fish.Fish.TryGetValue(12814, out f) )
                f.CatchData = new CatchData(3, 00){ BaitOrder = new(){ 12712 } };
            if( fish.Fish.TryGetValue(12815, out f) )
                f.CatchData = new CatchData(3, 00){ CurrentWeather = new(){ 9 }, BaitOrder = new(){ 12710, 12776 } };
            if( fish.Fish.TryGetValue(12816, out f) )
                f.CatchData = new CatchData(3, 00, 20, 3){ BaitOrder = new(){ 12710, 12776 } };
            if( fish.Fish.TryGetValue(12821, out f) )
                f.CatchData = new CatchData(3, 00, 9, 16){ BaitOrder = new(){ 12712 } };
            if( fish.Fish.TryGetValue(12825, out f) )
                f.CatchData = new CatchData(3, 00){ Folklore = 2502, BaitOrder = new(){ 12712, 12805 } };
            if( fish.Fish.TryGetValue(12826, out f) )
                f.CatchData = new CatchData(3, 00){ BaitOrder = new(){ 12709, 12754 } };
            if( fish.Fish.TryGetValue(12827, out f) )
                f.CatchData = new CatchData(3, 00){ Folklore = 2502, CurrentWeather = new(){ 9 }, BaitOrder = new(){ 12710, 12776 } };
            if( fish.Fish.TryGetValue(12828, out f) )
                f.CatchData = new CatchData(3, 00, 22, 4){ BaitOrder = new(){ 12704, 12722 } };
            if( fish.Fish.TryGetValue(12829, out f) )
                f.CatchData = new CatchData(3, 00){ CurrentWeather = new(){ 2, 1 }, BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(12830, out f) )
                f.CatchData = new CatchData(3, 00, 13, 20){ Folklore = 2501, BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(12831, out f) )
                f.CatchData = new CatchData(3, 00, 15, 18){ BaitOrder = new(){ 12707, 12730 } };
            if( fish.Fish.TryGetValue(12832, out f) )
                f.CatchData = new CatchData(3, 00, 9, 16){ Folklore = 2501, CurrentWeather = new(){ 2, 1 }, BaitOrder = new(){ 12709, 12754 } };
            if( fish.Fish.TryGetValue(12833, out f) )
                f.CatchData = new CatchData(3, 00, 15, 18){ BaitOrder = new(){ 12708 } };
            if( fish.Fish.TryGetValue(12834, out f) )
                f.CatchData = new CatchData(3, 00){ BaitOrder = new(){ 12712 } };
            if( fish.Fish.TryGetValue(12835, out f) )
                f.CatchData = new CatchData(3, 00){ BaitOrder = new(){ 12712 } };
            if( fish.Fish.TryGetValue(12836, out f) )
                f.CatchData = new CatchData(3, 00){ Folklore = 2501, CurrentWeather = new(){ 2, 1 }, BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(12837, out f) )
                f.CatchData = new CatchData(3, 00, 0, 6){ Folklore = 2500, BaitOrder = new(){ 12704 } };
            if( fish.Fish.TryGetValue(12754, out f) )
                f.CatchData = new CatchData(3, 00){ BaitOrder = new(){ 12709 } };
            if( fish.Fish.TryGetValue(12722, out f) )
                f.CatchData = new CatchData(3, 00){ Snagging = false, BaitOrder = new(){ 12704 } };
            if( fish.Fish.TryGetValue(12716, out f) )
                f.CatchData = new CatchData(3, 00){ Snagging = false, BaitOrder = new(){ 12712 } };
            if( fish.Fish.TryGetValue(12730, out f) )
                f.CatchData = new CatchData(3, 00){ Snagging = false, BaitOrder = new(){ 12707 } };
            if( fish.Fish.TryGetValue(12776, out f) )
                f.CatchData = new CatchData(3, 00){ Snagging = false, BaitOrder = new(){ 12710 } };
            if( fish.Fish.TryGetValue(12780, out f) )
                f.CatchData = new CatchData(3, 00){ Snagging = false, BaitOrder = new(){ 12706 } };
            if( fish.Fish.TryGetValue(12777, out f) )
                f.CatchData = new CatchData(3, 00){ Snagging = false, BaitOrder = new(){ 12705 } };
            if( fish.Fish.TryGetValue(13727, out f) )
                f.CatchData = new CatchData(3, 10){ Folklore = 2502, CurrentWeather = new(){ 9 }, BaitOrder = new(){ 12710 } };
            if( fish.Fish.TryGetValue(13728, out f) )
                f.CatchData = new CatchData(3, 10, 0, 2){ Folklore = 2500, CurrentWeather = new(){ 16 }, BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(13729, out f) )
                f.CatchData = new CatchData(3, 10){ BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(13730, out f) )
                f.CatchData = new CatchData(3, 10, 0, 6){ Snagging = true, Folklore = 2502, CurrentWeather = new(){ 1, 2 }, BaitOrder = new(){ 12712 } };
            if( fish.Fish.TryGetValue(13731, out f) )
                f.CatchData = new CatchData(3, 10, 15, 17){ Folklore = 2501, CurrentWeather = new(){ 2, 1 }, BaitOrder = new(){ 12712 } };
            if( fish.Fish.TryGetValue(13732, out f) )
                f.CatchData = new CatchData(3, 10, 0, 5){ Folklore = 2501, CurrentWeather = new(){ 3, 4 }, BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(14211, out f) )
                f.CatchData = new CatchData(3, 20, 6, 12){ Folklore = 2501, BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(14212, out f) )
                f.CatchData = new CatchData(3, 20){ Folklore = 2501, BaitOrder = new(){ 12705 } };
            if( fish.Fish.TryGetValue(14213, out f) )
                f.CatchData = new CatchData(3, 20){ Folklore = 2502, CurrentWeather = new(){ 2, 1 }, BaitOrder = new(){ 12712 } };
            if( fish.Fish.TryGetValue(14216, out f) )
                f.CatchData = new CatchData(3, 20){ Folklore = 2500, BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(14217, out f) )
                f.CatchData = new CatchData(3, 20){ Snagging = true, Folklore = 2500, BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(14218, out f) )
                f.CatchData = new CatchData(3, 20, 18, 23){ Folklore = 2502, CurrentWeather = new(){ 9 }, BaitOrder = new(){ 12710 } };
            if( fish.Fish.TryGetValue(14219, out f) )
                f.CatchData = new CatchData(3, 20){ Folklore = 2500, BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(14220, out f) )
                f.CatchData = new CatchData(3, 20){ Folklore = 2501, CurrentWeather = new(){ 4, 3 }, BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(15439, out f) )
                f.CatchData = new CatchData(3, 30){ Snagging = true, BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(15626, out f) )
                f.CatchData = new CatchData(3, 30){ BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(15627, out f) )
                f.CatchData = new CatchData(3, 30){ Snagging = true, Folklore = 2500, BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(15628, out f) )
                f.CatchData = new CatchData(3, 30){ Folklore = 2500, BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(15629, out f) )
                f.CatchData = new CatchData(3, 30){ Folklore = 2501, CurrentWeather = new(){ 11, 4, 3 }, BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(15630, out f) )
                f.CatchData = new CatchData(3, 30, 22, 2){ Folklore = 2501, CurrentWeather = new(){ 11, 4, 3 }, BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(15631, out f) )
                f.CatchData = new CatchData(3, 30){ Folklore = 2501, BaitOrder = new(){ 12709, 12754 } };
            if( fish.Fish.TryGetValue(15632, out f) )
                f.CatchData = new CatchData(3, 30){ BaitOrder = new(){ 12706, 12780 } };
            if( fish.Fish.TryGetValue(15633, out f) )
                f.CatchData = new CatchData(3, 30, 8, 10){ Folklore = 2501, BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(15634, out f) )
                f.CatchData = new CatchData(3, 30, 4, 6){ Folklore = 2501, CurrentWeather = new(){ 1, 2 }, BaitOrder = new(){ 12712 } };
            if( fish.Fish.TryGetValue(15635, out f) )
                f.CatchData = new CatchData(3, 30, 14, 16){ Folklore = 2502, BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(15636, out f) )
                f.CatchData = new CatchData(3, 30){ Folklore = 2502, CurrentWeather = new(){ 5 }, BaitOrder = new(){ 12712 } };
            if( fish.Fish.TryGetValue(15637, out f) )
                f.CatchData = new CatchData(3, 30){ BaitOrder = new(){ 12710, 12776 } };
            if( fish.Fish.TryGetValue(15638, out f) )
                f.CatchData = new CatchData(3, 30, 22, 2){ Folklore = 2502, CurrentWeather = new(){ 3 }, BaitOrder = new(){ 12705, 12777 } };
            if( fish.Fish.TryGetValue(16742, out f) )
                f.CatchData = new CatchData(3, 40){ Folklore = 2501, PreviousWeather = new(){ 1 }, CurrentWeather = new(){ 6 }, BaitOrder = new(){ 12712, 12805 } };
            if( fish.Fish.TryGetValue(16743, out f) )
                f.CatchData = new CatchData(3, 40){ Folklore = 2502, PreviousWeather = new(){ 4 }, CurrentWeather = new(){ 1 }, BaitOrder = new(){ 12708, 12753 } };
            if( fish.Fish.TryGetValue(16744, out f) )
                f.CatchData = new CatchData(3, 40){ Folklore = 2502, PreviousWeather = new(){ 3 }, CurrentWeather = new(){ 9 }, BaitOrder = new(){ 12710, 12776 } };
            if( fish.Fish.TryGetValue(16745, out f) )
                f.CatchData = new CatchData(3, 40){ PreviousWeather = new(){ 15 }, CurrentWeather = new(){ 16 }, BaitOrder = new(){ 12708, 12724 } };
            if( fish.Fish.TryGetValue(16746, out f) )
                f.CatchData = new CatchData(3, 40, 10, 16){ CurrentWeather = new(){ 16 }, BaitOrder = new(){ 12705, 12715 } };
            if( fish.Fish.TryGetValue(16747, out f) )
                f.CatchData = new CatchData(3, 40, 12, 16){ Folklore = 2501, PreviousWeather = new(){ 3, 11 }, CurrentWeather = new(){ 1 }, BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(16748, out f) )
                f.CatchData = new CatchData(3, 40, 21, 2){ Folklore = 2501, CurrentWeather = new(){ 1 }, BaitOrder = new(){ 12705, 12757 } };
            if( fish.Fish.TryGetValue(16749, out f) )
                f.CatchData = new CatchData(3, 40, 10, 13){ Folklore = 2501, PreviousWeather = new(){ 6 }, CurrentWeather = new(){ 1 }, BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(16750, out f) )
                f.CatchData = new CatchData(3, 40, 10, 13){ Folklore = 2502, CurrentWeather = new(){ 2, 1 }, BaitOrder = new(){ 12712 } };
            if( fish.Fish.TryGetValue(16751, out f) )
                f.CatchData = new CatchData(3, 40, 2, 6){ Folklore = 2501, BaitOrder = new(){ 12709 } };
            if( fish.Fish.TryGetValue(16752, out f) )
                f.CatchData = new CatchData(3, 40){ Folklore = 2502, BaitOrder = new(){ 12710, 12776 } };
            if( fish.Fish.TryGetValue(16753, out f) )
                f.CatchData = new CatchData(3, 40, 6, 10){ Folklore = 2502, BaitOrder = new(){ 12705, 12777 } };
            if( fish.Fish.TryGetValue(16754, out f) )
                f.CatchData = new CatchData(3, 40, 2, 6){ Folklore = 2501, PreviousWeather = new(){ 7 }, CurrentWeather = new(){ 8 }, BaitOrder = new(){ 12711, 12780 } };
            if( fish.Fish.TryGetValue(16756, out f) )
                f.CatchData = new CatchData(3, 40){ Folklore = 2500, PreviousWeather = new(){ 15, 16 }, CurrentWeather = new(){ 16 }, BaitOrder = new(){ 2607, 12715 } };
            if( fish.Fish.TryGetValue(17577, out f) )
                f.CatchData = new CatchData(3, 50, 10, 14){ Snagging = false, Folklore = 2500, CurrentWeather = new(){ 1 }, BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(17578, out f) )
                f.CatchData = new CatchData(3, 50){ Snagging = false, PreviousWeather = new(){ 15 }, CurrentWeather = new(){ 16 }, BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(17579, out f) )
                f.CatchData = new CatchData(3, 50, 8, 12){ Snagging = false, Folklore = 2502, PreviousWeather = new(){ 4 }, CurrentWeather = new(){ 1 }, BaitOrder = new(){ 12707, 12730 } };
            if( fish.Fish.TryGetValue(17580, out f) )
                f.CatchData = new CatchData(3, 50, 18, 22){ Snagging = false, Folklore = 2502, PreviousWeather = new(){ 3 }, CurrentWeather = new(){ 9 }, BaitOrder = new(){ 12710 } };
            if( fish.Fish.TryGetValue(17581, out f) )
                f.CatchData = new CatchData(3, 50){ Snagging = false, Folklore = 2502, PreviousWeather = new(){ 2 }, CurrentWeather = new(){ 9 }, BaitOrder = new(){ 12712, 12805 } };
            if( fish.Fish.TryGetValue(17582, out f) )
                f.CatchData = new CatchData(3, 50, 6, 8){ Snagging = false, Folklore = 2501, PreviousWeather = new(){ 11, 3, 4 }, CurrentWeather = new(){ 9 }, BaitOrder = new(){ 12704, 12722 } };
            if( fish.Fish.TryGetValue(17583, out f) )
                f.CatchData = new CatchData(3, 50, 8, 16){ Snagging = false, Folklore = 2501, PreviousWeather = new(){ 1 }, CurrentWeather = new(){ 1 }, BaitOrder = new(){ 12709, 12754 } };
            if( fish.Fish.TryGetValue(17584, out f) )
                f.CatchData = new CatchData(3, 50, 8, 16){ Snagging = false, Folklore = 2501, PreviousWeather = new(){ 4, 11, 3 }, CurrentWeather = new(){ 1, 2 }, BaitOrder = new(){ 12709, 12754 } };
            if( fish.Fish.TryGetValue(17585, out f) )
                f.CatchData = new CatchData(3, 50){ Snagging = false, Folklore = 2501, PreviousWeather = new(){ 3, 4 }, CurrentWeather = new(){ 8 }, BaitOrder = new(){ 12704, 12757 } };
            if( fish.Fish.TryGetValue(17586, out f) )
                f.CatchData = new CatchData(3, 50){ Snagging = false, Folklore = 2501, PreviousWeather = new(){ 3 }, CurrentWeather = new(){ 6 }, BaitOrder = new(){ 12711 } };
            if( fish.Fish.TryGetValue(17587, out f) )
                f.CatchData = new CatchData(3, 50){ Snagging = false, Folklore = 2501, PreviousWeather = new(){ 1 }, CurrentWeather = new(){ 6 }, BaitOrder = new(){ 12712 } };
            if( fish.Fish.TryGetValue(17588, out f) )
                f.CatchData = new CatchData(3, 50, 10, 15){ Snagging = false, Folklore = 2501, Predator = (12800, 3), CurrentWeather = new(){ 1, 2 }, BaitOrder = new(){ 12709, 12754 } };
            if( fish.Fish.TryGetValue(17589, out f) )
                f.CatchData = new CatchData(3, 50){ Snagging = false, Folklore = 2502, Predator = (13727, 3), CurrentWeather = new(){ 9 }, BaitOrder = new(){ 12710, 12776 } };
            if( fish.Fish.TryGetValue(17590, out f) )
                f.CatchData = new CatchData(3, 50, 1, 4){ Snagging = false, Folklore = 2501, Predator = (12757, 6), CurrentWeather = new(){ 1 }, BaitOrder = new(){ 12705, 12757 } };
            if( fish.Fish.TryGetValue(17591, out f) )
                f.CatchData = new CatchData(3, 50, 5, 7){ Snagging = false, Folklore = 2502, Predator = (12810, 3), CurrentWeather = new(){ 1 }, BaitOrder = new(){ 12712 } };
            if( fish.Fish.TryGetValue(17592, out f) )
                f.CatchData = new CatchData(3, 50, 0, 3){ Snagging = false, Folklore = 2500, Predator = (12715, 5), CurrentWeather = new(){ 16 }, BaitOrder = new(){ 12705, 12715 } };
            if( fish.Fish.TryGetValue(17593, out f) )
                f.CatchData = new CatchData(3, 50, 5, 8){ Snagging = false, Folklore = 2501, Predator = (12805, 5), CurrentWeather = new(){ 6 }, BaitOrder = new(){ 12712, 12805 } };
            if( fish.Fish.TryGetValue(14215, out f) )
                f.CatchData = new CatchData(3, 20){ Snagging = true, Folklore = 2501, CurrentWeather = new(){ 1, 2 }, BaitOrder = new(){ 12709 } };
            if( fish.Fish.TryGetValue(20018, out f) )
                f.CatchData = new CatchData(4, 00){ Folklore = 2505, CurrentWeather = new(){ 9 }, BaitOrder = new(){ 20617, 20112 } };
            if( fish.Fish.TryGetValue(20020, out f) )
                f.CatchData = new CatchData(4, 00){ BaitOrder = new(){ 20614 } };
            if( fish.Fish.TryGetValue(20021, out f) )
                f.CatchData = new CatchData(4, 00, 16){ BaitOrder = new(){ 20619 } };
            if( fish.Fish.TryGetValue(20022, out f) )
                f.CatchData = new CatchData(4, 00){ BaitOrder = new(){ 20676 } };
            if( fish.Fish.TryGetValue(20024, out f) )
                f.CatchData = new CatchData(4, 00){ Folklore = 2505, BaitOrder = new(){ 20675 } };
            if( fish.Fish.TryGetValue(20027, out f) )
                f.CatchData = new CatchData(4, 00, 20, 4){ BaitOrder = new(){ 20615 } };
            if( fish.Fish.TryGetValue(20030, out f) )
                f.CatchData = new CatchData(4, 00, 0, 4){ Folklore = 2505, CurrentWeather = new(){ 7 }, BaitOrder = new(){ 20675 } };
            if( fish.Fish.TryGetValue(20034, out f) )
                f.CatchData = new CatchData(4, 00, 10, 19){ CurrentWeather = new(){ 1 }, BaitOrder = new(){ 20676 } };
            if( fish.Fish.TryGetValue(20036, out f) )
                f.CatchData = new CatchData(4, 00){ CurrentWeather = new(){ 1 }, BaitOrder = new(){ 20615 } };
            if( fish.Fish.TryGetValue(20040, out f) )
                f.CatchData = new CatchData(4, 00, 12, 18){ Folklore = 2503, CurrentWeather = new(){ 10 }, BaitOrder = new(){ 20616, 20025 } };
            if( fish.Fish.TryGetValue(20043, out f) )
                f.CatchData = new CatchData(4, 00){ CurrentWeather = new(){ 1 }, BaitOrder = new(){ 20619 } };
            if( fish.Fish.TryGetValue(20048, out f) )
                f.CatchData = new CatchData(4, 00){ Folklore = 2505, BaitOrder = new(){ 20676 } };
            if( fish.Fish.TryGetValue(20051, out f) )
                f.CatchData = new CatchData(4, 00){ Folklore = 2505, CurrentWeather = new(){ 7 }, BaitOrder = new(){ 20675 } };
            if( fish.Fish.TryGetValue(20054, out f) )
                f.CatchData = new CatchData(4, 00){ Folklore = 2505, BaitOrder = new(){ 20617, 20112 } };
            if( fish.Fish.TryGetValue(20058, out f) )
                f.CatchData = new CatchData(4, 00){ CurrentWeather = new(){ 3, 4 }, BaitOrder = new(){ 20614, 20064 } };
            if( fish.Fish.TryGetValue(20074, out f) )
                f.CatchData = new CatchData(4, 00){ CurrentWeather = new(){ 5 }, BaitOrder = new(){ 20675 } };
            if( fish.Fish.TryGetValue(20076, out f) )
                f.CatchData = new CatchData(4, 00){ CurrentWeather = new(){ 4, 5, 3, 11 }, BaitOrder = new(){ 20615 } };
            if( fish.Fish.TryGetValue(20084, out f) )
                f.CatchData = new CatchData(4, 00){ CurrentWeather = new(){ 3, 4 }, BaitOrder = new(){ 20613 } };
            if( fish.Fish.TryGetValue(20085, out f) )
                f.CatchData = new CatchData(4, 00, 16, 19){ BaitOrder = new(){ 20613 } };
            if( fish.Fish.TryGetValue(20086, out f) )
                f.CatchData = new CatchData(4, 00){ CurrentWeather = new(){ 3, 4 }, BaitOrder = new(){ 20613 } };
            if( fish.Fish.TryGetValue(20100, out f) )
                f.CatchData = new CatchData(4, 00, 8, 16){ Folklore = 2505, CurrentWeather = new(){ 3, 5 }, BaitOrder = new(){ 20676 } };
            if( fish.Fish.TryGetValue(20104, out f) )
                f.CatchData = new CatchData(4, 00){ BaitOrder = new(){ 20617, 20112 } };
            if( fish.Fish.TryGetValue(20105, out f) )
                f.CatchData = new CatchData(4, 00){ BaitOrder = new(){ 20617, 20112 } };
            if( fish.Fish.TryGetValue(20114, out f) )
                f.CatchData = new CatchData(4, 00){ CurrentWeather = new(){ 7, 8 }, BaitOrder = new(){ 20619 } };
            if( fish.Fish.TryGetValue(20120, out f) )
                f.CatchData = new CatchData(4, 00){ BaitOrder = new(){ 20619 } };
            if( fish.Fish.TryGetValue(20122, out f) )
                f.CatchData = new CatchData(4, 00, 10, 18){ BaitOrder = new(){ 20615 } };
            if( fish.Fish.TryGetValue(20128, out f) )
                f.CatchData = new CatchData(4, 00){ BaitOrder = new(){ 20615 } };
            if( fish.Fish.TryGetValue(20141, out f) )
                f.CatchData = new CatchData(4, 00){ CurrentWeather = new(){ 1, 2 }, BaitOrder = new(){ 20619 } };
            if( fish.Fish.TryGetValue(20142, out f) )
                f.CatchData = new CatchData(4, 00, 0, 4){ Folklore = 2503, CurrentWeather = new(){ 4 }, BaitOrder = new(){ 20675 } };
            if( fish.Fish.TryGetValue(20143, out f) )
                f.CatchData = new CatchData(4, 00){ CurrentWeather = new(){ 7, 8 }, BaitOrder = new(){ 2585, 4869, 4904 } };
            if( fish.Fish.TryGetValue(20524, out f) )
                f.CatchData = new CatchData(4, 00, 9, 16){ BaitOrder = new(){ 2585, 4869, 4904 } };
            if( fish.Fish.TryGetValue(20112, out f) )
                f.CatchData = new CatchData(4, 00){ Snagging = false, BaitOrder = new(){ 20617 } };
            if( fish.Fish.TryGetValue(20025, out f) )
                f.CatchData = new CatchData(4, 00){ Snagging = false, BaitOrder = new(){ 20616 } };
            if( fish.Fish.TryGetValue(20056, out f) )
                f.CatchData = new CatchData(4, 00){ Snagging = false, BaitOrder = new(){ 20615 } };
            if( fish.Fish.TryGetValue(20127, out f) )
                f.CatchData = new CatchData(4, 00){ Snagging = false, BaitOrder = new(){ 20614 } };
            if( fish.Fish.TryGetValue(20064, out f) )
                f.CatchData = new CatchData(4, 00){ Snagging = false, BaitOrder = new(){ 20613 } };
            if( fish.Fish.TryGetValue(20785, out f) )
                f.CatchData = new CatchData(4, 10){ BaitOrder = new(){ 20613 } };
            if( fish.Fish.TryGetValue(20786, out f) )
                f.CatchData = new CatchData(4, 10){ Snagging = false, BaitOrder = new(){ 20617 } };
            if( fish.Fish.TryGetValue(20787, out f) )
                f.CatchData = new CatchData(4, 10){ BaitOrder = new(){ 20614 } };
            if( fish.Fish.TryGetValue(20788, out f) )
                f.CatchData = new CatchData(4, 10){ BaitOrder = new(){ 20614 } };
            if( fish.Fish.TryGetValue(20789, out f) )
                f.CatchData = new CatchData(4, 10){ BaitOrder = new(){ 20615 } };
            if( fish.Fish.TryGetValue(21174, out f) )
                f.CatchData = new CatchData(4, 10, 19, 23){ Folklore = 2503, CurrentWeather = new(){ 3, 4 }, BaitOrder = new(){ 20619 } };
            if( fish.Fish.TryGetValue(21175, out f) )
                f.CatchData = new CatchData(4, 10, 12, 16){ Snagging = false, Folklore = 2503, CurrentWeather = new(){ 3, 4, 5, 11 }, BaitOrder = new(){ 20619 } };
            if( fish.Fish.TryGetValue(21176, out f) )
                f.CatchData = new CatchData(4, 10, 8, 12){ Folklore = 2505, CurrentWeather = new(){ 5 }, BaitOrder = new(){ 20676 } };
            if( fish.Fish.TryGetValue(21177, out f) )
                f.CatchData = new CatchData(4, 10, 0, 4){ Folklore = 2505, BaitOrder = new(){ 20675 } };
            if( fish.Fish.TryGetValue(21178, out f) )
                f.CatchData = new CatchData(4, 10, 16, 20){ Folklore = 2505, CurrentWeather = new(){ 2, 1 }, BaitOrder = new(){ 20675 } };
            if( fish.Fish.TryGetValue(21179, out f) )
                f.CatchData = new CatchData(4, 10, 2, 12){ };
            if( fish.Fish.TryGetValue(22398, out f) )
                f.CatchData = new CatchData(4, 20){ Snagging = false, Folklore = 2505, CurrentWeather = new(){ 3, 4 }, BaitOrder = new(){ 20614, 20127 } };
            if( fish.Fish.TryGetValue(22397, out f) )
                f.CatchData = new CatchData(4, 20, 4, 8){ Folklore = 2505, BaitOrder = new(){ 20675 } };
            if( fish.Fish.TryGetValue(22396, out f) )
                f.CatchData = new CatchData(4, 20){ Folklore = 2505, BaitOrder = new(){ 20617, 20112 } };
            if( fish.Fish.TryGetValue(22395, out f) )
                f.CatchData = new CatchData(4, 20){ Folklore = 2505, BaitOrder = new(){ 20614 } };
            if( fish.Fish.TryGetValue(22394, out f) )
                f.CatchData = new CatchData(4, 20){ Folklore = 2503, CurrentWeather = new(){ 3, 4 }, BaitOrder = new(){ 20616, 20025 } };
            if( fish.Fish.TryGetValue(22393, out f) )
                f.CatchData = new CatchData(4, 20){ Folklore = 2505, BaitOrder = new(){ 20617, 20112 } };
            if( fish.Fish.TryGetValue(22392, out f) )
                f.CatchData = new CatchData(4, 20){ Folklore = 2503, CurrentWeather = new(){ 3, 4 }, BaitOrder = new(){ 20613, 20064 } };
            if( fish.Fish.TryGetValue(22391, out f) )
                f.CatchData = new CatchData(4, 20){ Folklore = 2503, CurrentWeather = new(){ 2, 1 }, BaitOrder = new(){ 20619 } };
            if( fish.Fish.TryGetValue(22390, out f) )
                f.CatchData = new CatchData(4, 20, 5, 7){ Snagging = false, Folklore = 2505, BaitOrder = new(){ 20676 } };
            if( fish.Fish.TryGetValue(22389, out f) )
                f.CatchData = new CatchData(4, 20, 4, 8){ Folklore = 2503, BaitOrder = new(){ 20675 } };
            if( fish.Fish.TryGetValue(23055, out f) )
                f.CatchData = new CatchData(4, 30, 4, 8){ Folklore = 2505, BaitOrder = new(){ 20675, 22397 } };
            if( fish.Fish.TryGetValue(23056, out f) )
                f.CatchData = new CatchData(4, 30, 4, 8){ Folklore = 2505, BaitOrder = new(){ 20675, 22397 } };
            if( fish.Fish.TryGetValue(23057, out f) )
                f.CatchData = new CatchData(4, 30){ Folklore = 2503, BaitOrder = new(){ 20615, 20056 } };
            if( fish.Fish.TryGetValue(23058, out f) )
                f.CatchData = new CatchData(4, 30){ Folklore = 2503, CurrentWeather = new(){ 9 }, BaitOrder = new(){ 20675 } };
            if( fish.Fish.TryGetValue(23059, out f) )
                f.CatchData = new CatchData(4, 30, 12, 16){ Folklore = 2503, BaitOrder = new(){ 20619 } };
            if( fish.Fish.TryGetValue(23060, out f) )
                f.CatchData = new CatchData(4, 30){ Folklore = 2503, CurrentWeather = new(){ 11 }, BaitOrder = new(){ 20619 } };
            if( fish.Fish.TryGetValue(23061, out f) )
                f.CatchData = new CatchData(4, 30, 8, 12){ Folklore = 2503, BaitOrder = new(){ 20615, 20056 } };
            if( fish.Fish.TryGetValue(23062, out f) )
                f.CatchData = new CatchData(4, 30){ Folklore = 2503, CurrentWeather = new(){ 1 }, BaitOrder = new(){ 20616, 20025 } };
            if( fish.Fish.TryGetValue(23063, out f) )
                f.CatchData = new CatchData(4, 30){ Folklore = 2505, BaitOrder = new(){ 20617, 20112 } };
            if( fish.Fish.TryGetValue(23064, out f) )
                f.CatchData = new CatchData(4, 30){ Folklore = 2505, BaitOrder = new(){ 20676 } };
            if( fish.Fish.TryGetValue(23065, out f) )
                f.CatchData = new CatchData(4, 30, 16, 20){ Folklore = 2505, BaitOrder = new(){ 20676 } };
            if( fish.Fish.TryGetValue(23066, out f) )
                f.CatchData = new CatchData(4, 30){ Folklore = 2505, CurrentWeather = new(){ 9 }, BaitOrder = new(){ 20617, 20112 } };
            if( fish.Fish.TryGetValue(23067, out f) )
                f.CatchData = new CatchData(4, 30, 12, 14){ Folklore = 2505, BaitOrder = new(){ 20675 } };
            if( fish.Fish.TryGetValue(23068, out f) )
                f.CatchData = new CatchData(4, 30){ Folklore = 2505, CurrentWeather = new(){ 4 }, BaitOrder = new(){ 20675 } };
            if( fish.Fish.TryGetValue(23069, out f) )
                f.CatchData = new CatchData(4, 30, 20){ Folklore = 2505, BaitOrder = new(){ 20675 } };
            if( fish.Fish.TryGetValue(23070, out f) )
                f.CatchData = new CatchData(4, 30){ Folklore = 2505, CurrentWeather = new(){ 5 }, BaitOrder = new(){ 20675 } };
            if( fish.Fish.TryGetValue(24203, out f) )
                f.CatchData = new CatchData(4, 40, 0, 4){ Folklore = 2505, BaitOrder = new(){ 20675, 21177 } };
            if( fish.Fish.TryGetValue(24204, out f) )
                f.CatchData = new CatchData(4, 40, 0, 16){ Folklore = 2505, PreviousWeather = new(){ 2 }, CurrentWeather = new(){ 2, 1 }, BaitOrder = new(){ 20675 } };
            if( fish.Fish.TryGetValue(24205, out f) )
                f.CatchData = new CatchData(4, 40){ Folklore = 2503, CurrentWeather = new(){ 9 }, BaitOrder = new(){ 20615, 20056 } };
            if( fish.Fish.TryGetValue(24206, out f) )
                f.CatchData = new CatchData(4, 40, 8, 12){ Folklore = 2503, CurrentWeather = new(){ 3 }, BaitOrder = new(){ 20613, 20064 } };
            if( fish.Fish.TryGetValue(24207, out f) )
                f.CatchData = new CatchData(4, 40, 16, 20){ Folklore = 2503, BaitOrder = new(){ 20675 } };
            if( fish.Fish.TryGetValue(24208, out f) )
                f.CatchData = new CatchData(4, 40){ Folklore = 2503, CurrentWeather = new(){ 4 }, BaitOrder = new(){ 20675 } };
            if( fish.Fish.TryGetValue(24209, out f) )
                f.CatchData = new CatchData(4, 40, 20){ Folklore = 2503, CurrentWeather = new(){ 1 }, BaitOrder = new(){ 20619 } };
            if( fish.Fish.TryGetValue(24210, out f) )
                f.CatchData = new CatchData(4, 40){ Folklore = 2503, PreviousWeather = new(){ 4 }, CurrentWeather = new(){ 2 }, BaitOrder = new(){ 20613 } };
            if( fish.Fish.TryGetValue(24211, out f) )
                f.CatchData = new CatchData(4, 40, 16, 20){ Folklore = 2503, CurrentWeather = new(){ 3 }, BaitOrder = new(){ 20619 } };
            if( fish.Fish.TryGetValue(24212, out f) )
                f.CatchData = new CatchData(4, 40){ Folklore = 2503, PreviousWeather = new(){ 3, 5, 4, 11 }, CurrentWeather = new(){ 1 }, BaitOrder = new(){ 20619 } };
            if( fish.Fish.TryGetValue(24213, out f) )
                f.CatchData = new CatchData(4, 40, 4, 6){ Folklore = 2505, BaitOrder = new(){ 20614 } };
            if( fish.Fish.TryGetValue(24214, out f) )
                f.CatchData = new CatchData(4, 40, 0, 8){ Folklore = 2505, PreviousWeather = new(){ 9 }, CurrentWeather = new(){ 3 }, BaitOrder = new(){ 20676 } };
            if( fish.Fish.TryGetValue(24215, out f) )
                f.CatchData = new CatchData(4, 40){ Folklore = 2505, CurrentWeather = new(){ 5 }, BaitOrder = new(){ 20617, 20112 } };
            if( fish.Fish.TryGetValue(24216, out f) )
                f.CatchData = new CatchData(4, 40){ Folklore = 2505, PreviousWeather = new(){ 3 }, CurrentWeather = new(){ 2, 1 }, BaitOrder = new(){ 20675 } };
            if( fish.Fish.TryGetValue(24217, out f) )
                f.CatchData = new CatchData(4, 40, 8, 16){ Folklore = 2505, PreviousWeather = new(){ 7, 6 }, CurrentWeather = new(){ 3 }, BaitOrder = new(){ 20614, 20127 } };
            if( fish.Fish.TryGetValue(24218, out f) )
                f.CatchData = new CatchData(4, 40, 4, 8){ Folklore = 2505, CurrentWeather = new(){ 4 }, BaitOrder = new(){ 20619 } };
            if( fish.Fish.TryGetValue(24881, out f) )
                f.CatchData = new CatchData(4, 50){ Folklore = 2503, PreviousWeather = new(){ 1 }, CurrentWeather = new(){ 2 }, BaitOrder = new(){ 20619 } };
            if( fish.Fish.TryGetValue(24882, out f) )
                f.CatchData = new CatchData(4, 50, 20){ Folklore = 2505, CurrentWeather = new(){ 9 }, BaitOrder = new(){ 20617, 20112 } };
            if( fish.Fish.TryGetValue(24883, out f) )
                f.CatchData = new CatchData(4, 50, 10, 18){ Folklore = 2505, PreviousWeather = new(){ 9 }, CurrentWeather = new(){ 2 }, BaitOrder = new(){ 20676 } };
            if( fish.Fish.TryGetValue(24884, out f) )
                f.CatchData = new CatchData(4, 50, 16){ Folklore = 2505, PreviousWeather = new(){ 2, 1 }, CurrentWeather = new(){ 9 }, BaitOrder = new(){ 20617 } };
            if( fish.Fish.TryGetValue(24885, out f) )
                f.CatchData = new CatchData(4, 50){ Folklore = 2505, CurrentWeather = new(){ 7 }, BaitOrder = new(){ 20614 } };
            if( fish.Fish.TryGetValue(24886, out f) )
                f.CatchData = new CatchData(4, 50){ Folklore = 2505, PreviousWeather = new(){ 2 }, CurrentWeather = new(){ 1 }, BaitOrder = new(){ 20619 } };
            if( fish.Fish.TryGetValue(24887, out f) )
                f.CatchData = new CatchData(4, 50, 20){ Folklore = 2505, CurrentWeather = new(){ 1 }, BaitOrder = new(){ 20675 } };
            if( fish.Fish.TryGetValue(24888, out f) )
                f.CatchData = new CatchData(4, 50, 16){ Folklore = 2505, PreviousWeather = new(){ 2 }, CurrentWeather = new(){ 8 }, BaitOrder = new(){ 20675 } };
            if( fish.Fish.TryGetValue(24889, out f) )
                f.CatchData = new CatchData(4, 50, 0, 8){ Folklore = 2505, PreviousWeather = new(){ 3 }, CurrentWeather = new(){ 2 }, BaitOrder = new(){ 20675 } };
            if( fish.Fish.TryGetValue(24890, out f) )
                f.CatchData = new CatchData(4, 50, 12, 16){ Folklore = 2505, CurrentWeather = new(){ 4 }, BaitOrder = new(){ 20619 } };
            if( fish.Fish.TryGetValue(24891, out f) )
                f.CatchData = new CatchData(4, 50, 4, 8){ Folklore = 2505, PreviousWeather = new(){ 7 }, CurrentWeather = new(){ 2, 1 }, BaitOrder = new(){ 20619 } };
            if( fish.Fish.TryGetValue(24892, out f) )
                f.CatchData = new CatchData(4, 50, 8, 12){ Folklore = 2505, PreviousWeather = new(){ 2 }, CurrentWeather = new(){ 1 }, BaitOrder = new(){ 20615 } };
            if( fish.Fish.TryGetValue(24893, out f) )
                f.CatchData = new CatchData(4, 50, 0, 8){ Folklore = 2505, PreviousWeather = new(){ 2 }, CurrentWeather = new(){ 4 }, BaitOrder = new(){ 20614, 20127 } };
            if( fish.Fish.TryGetValue(24990, out f) )
                f.CatchData = new CatchData(4, 50, 16, 20){ Folklore = 2503, BaitOrder = new(){ 20675, 24207 } };
            if( fish.Fish.TryGetValue(24991, out f) )
                f.CatchData = new CatchData(4, 50){ Folklore = 2503, Predator = (23060, 2), CurrentWeather = new(){ 11 }, BaitOrder = new(){ 20619 } };
            if( fish.Fish.TryGetValue(24992, out f) )
                f.CatchData = new CatchData(4, 50, 16, 18){ Folklore = 2503, Predator = (20040, 2), BaitOrder = new(){ 20616, 20025 } };
            if( fish.Fish.TryGetValue(24993, out f) )
                f.CatchData = new CatchData(4, 50, 4, 8){ Folklore = 2505, PreviousWeather = new(){ 9 }, CurrentWeather = new(){ 3 }, BaitOrder = new(){ 20676, 24214 } };
            if( fish.Fish.TryGetValue(24994, out f) )
                f.CatchData = new CatchData(4, 50){ Folklore = 2505, Predator = (23056, 3), BaitOrder = new(){ 20675 } };
            if( fish.Fish.TryGetValue(24995, out f) )
                f.CatchData = new CatchData(4, 50, 5, 7){ Folklore = 2505, PreviousWeather = new(){ 7 }, CurrentWeather = new(){ 1 }, BaitOrder = new(){ 20675 } };
            if( fish.Fish.TryGetValue(27417, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(27411, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(27412, out f) )
                f.CatchData = new CatchData(5, 00){ Snagging = true, BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(27414, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(27413, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(27415, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(27416, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(27410, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(27418, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(26746, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(27425, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(27427, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(27426, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(27431, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(27433, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(27432, out f) )
                f.CatchData = new CatchData(5, 00){ Folklore = 2507, CurrentWeather = new(){ 1 }, BaitOrder = new(){ 27589 } };
            if( fish.Fish.TryGetValue(27428, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(27429, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(27430, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(27434, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(27435, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(27436, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27587 } };
            if( fish.Fish.TryGetValue(27437, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27587 } };
            if( fish.Fish.TryGetValue(27438, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(27439, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(27440, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(27441, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27587 } };
            if( fish.Fish.TryGetValue(27442, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27587 } };
            if( fish.Fish.TryGetValue(27443, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(27444, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(27445, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(27449, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27583 } };
            if( fish.Fish.TryGetValue(27448, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27583 } };
            if( fish.Fish.TryGetValue(27446, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27583 } };
            if( fish.Fish.TryGetValue(26747, out f) )
                f.CatchData = new CatchData(5, 00){ Snagging = false, BaitOrder = new(){ 27583 } };
            if( fish.Fish.TryGetValue(27451, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27588 } };
            if( fish.Fish.TryGetValue(27450, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27583 } };
            if( fish.Fish.TryGetValue(27447, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27583 } };
            if( fish.Fish.TryGetValue(27452, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27583 } };
            if( fish.Fish.TryGetValue(27453, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27583 } };
            if( fish.Fish.TryGetValue(27457, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27588 } };
            if( fish.Fish.TryGetValue(27454, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27588, 27457 } };
            if( fish.Fish.TryGetValue(27455, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27583 } };
            if( fish.Fish.TryGetValue(27456, out f) )
                f.CatchData = new CatchData(5, 00, 16){ Folklore = 2507, CurrentWeather = new(){ 4, 3 }, BaitOrder = new(){ 27588, 27457 } };
            if( fish.Fish.TryGetValue(27419, out f) )
                f.CatchData = new CatchData(5, 00){ Snagging = true, BaitOrder = new(){ 27583 } };
            if( fish.Fish.TryGetValue(27420, out f) )
                f.CatchData = new CatchData(5, 00){ Snagging = true, BaitOrder = new(){ 27583 } };
            if( fish.Fish.TryGetValue(27422, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27583 } };
            if( fish.Fish.TryGetValue(27421, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27583 } };
            if( fish.Fish.TryGetValue(27423, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27583 } };
            if( fish.Fish.TryGetValue(27424, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27583 } };
            if( fish.Fish.TryGetValue(27458, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27584 } };
            if( fish.Fish.TryGetValue(27459, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27584 } };
            if( fish.Fish.TryGetValue(27462, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27584 } };
            if( fish.Fish.TryGetValue(27460, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27584 } };
            if( fish.Fish.TryGetValue(27461, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27584 } };
            if( fish.Fish.TryGetValue(27465, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27584 } };
            if( fish.Fish.TryGetValue(27466, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27584, 27461 } };
            if( fish.Fish.TryGetValue(26749, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27584 } };
            if( fish.Fish.TryGetValue(27467, out f) )
                f.CatchData = new CatchData(5, 00, 10, 18){ Folklore = 2507, CurrentWeather = new(){ 14, 1, 2 }, BaitOrder = new(){ 27584, 27461 } };
            if( fish.Fish.TryGetValue(27463, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27584 } };
            if( fish.Fish.TryGetValue(27464, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27586 } };
            if( fish.Fish.TryGetValue(27470, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(27471, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(27475, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27585 } };
            if( fish.Fish.TryGetValue(27474, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27585 } };
            if( fish.Fish.TryGetValue(27479, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(27469, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27585 } };
            if( fish.Fish.TryGetValue(27476, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27585 } };
            if( fish.Fish.TryGetValue(27477, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27585 } };
            if( fish.Fish.TryGetValue(27472, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27585 } };
            if( fish.Fish.TryGetValue(27478, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27585 } };
            if( fish.Fish.TryGetValue(27468, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27585 } };
            if( fish.Fish.TryGetValue(27481, out f) )
                f.CatchData = new CatchData(5, 00){ Folklore = 2507, CurrentWeather = new(){ 1 }, BaitOrder = new(){ 27589 } };
            if( fish.Fish.TryGetValue(26748, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(27473, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27585 } };
            if( fish.Fish.TryGetValue(27480, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27587 } };
            if( fish.Fish.TryGetValue(27482, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(27486, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(27484, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27585 } };
            if( fish.Fish.TryGetValue(27490, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27587 } };
            if( fish.Fish.TryGetValue(27491, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27587, 27490 } };
            if( fish.Fish.TryGetValue(27487, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27585 } };
            if( fish.Fish.TryGetValue(27485, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27587 } };
            if( fish.Fish.TryGetValue(27492, out f) )
                f.CatchData = new CatchData(5, 00, 12, 20){ BaitOrder = new(){ 27587 } };
            if( fish.Fish.TryGetValue(27489, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27587 } };
            if( fish.Fish.TryGetValue(27488, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27587 } };
            if( fish.Fish.TryGetValue(27483, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27587 } };
            if( fish.Fish.TryGetValue(27493, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27587 } };
            if( fish.Fish.TryGetValue(27494, out f) )
                f.CatchData = new CatchData(5, 00, 0, 8){ Folklore = 2507, CurrentWeather = new(){ 4 }, BaitOrder = new(){ 27589 } };
            if( fish.Fish.TryGetValue(27497, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27583 } };
            if( fish.Fish.TryGetValue(27495, out f) )
                f.CatchData = new CatchData(5, 00){ Snagging = true, BaitOrder = new(){ 27588 } };
            if( fish.Fish.TryGetValue(27499, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27583 } };
            if( fish.Fish.TryGetValue(27501, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27588 } };
            if( fish.Fish.TryGetValue(27502, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27588 } };
            if( fish.Fish.TryGetValue(27504, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27588 } };
            if( fish.Fish.TryGetValue(27503, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27588 } };
            if( fish.Fish.TryGetValue(27496, out f) )
                f.CatchData = new CatchData(5, 00){ Snagging = true, BaitOrder = new(){ 27588 } };
            if( fish.Fish.TryGetValue(27500, out f) )
                f.CatchData = new CatchData(5, 00){ CurrentWeather = new(){ 2 }, BaitOrder = new(){ 27588 } };
            if( fish.Fish.TryGetValue(27505, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27588 } };
            if( fish.Fish.TryGetValue(27506, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27588 } };
            if( fish.Fish.TryGetValue(27507, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27588, 27506 } };
            if( fish.Fish.TryGetValue(27508, out f) )
                f.CatchData = new CatchData(5, 00, 12, 16){ Folklore = 2507, CurrentWeather = new(){ 1 }, BaitOrder = new(){ 27588, 27506 } };
            if( fish.Fish.TryGetValue(27509, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27588 } };
            if( fish.Fish.TryGetValue(27510, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27588 } };
            if( fish.Fish.TryGetValue(27511, out f) )
                f.CatchData = new CatchData(5, 00){ Snagging = true, BaitOrder = new(){ 27588 } };
            if( fish.Fish.TryGetValue(27498, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27588 } };
            if( fish.Fish.TryGetValue(27512, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27588 } };
            if( fish.Fish.TryGetValue(27513, out f) )
                f.CatchData = new CatchData(5, 00){ Snagging = true, BaitOrder = new(){ 27588 } };
            if( fish.Fish.TryGetValue(27514, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27588 } };
            if( fish.Fish.TryGetValue(27515, out f) )
                f.CatchData = new CatchData(5, 00){ BaitOrder = new(){ 27588 } };
            if( fish.Fish.TryGetValue(28065, out f) )
                f.CatchData = new CatchData(5, 10, 18, 21){ Folklore = 2507, BaitOrder = new(){ 27589 } };
            if( fish.Fish.TryGetValue(28189, out f) )
                f.CatchData = new CatchData(5, 10){ Snagging = false, BaitOrder = new(){ 28634 } };
            if( fish.Fish.TryGetValue(28067, out f) )
                f.CatchData = new CatchData(5, 10){ Folklore = 2507, CurrentWeather = new(){ 4 }, BaitOrder = new(){ 27589 } };
            if( fish.Fish.TryGetValue(28190, out f) )
                f.CatchData = new CatchData(5, 10){ BaitOrder = new(){ 28634 } };
            if( fish.Fish.TryGetValue(28068, out f) )
                f.CatchData = new CatchData(5, 10){ Folklore = 2507, CurrentWeather = new(){ 7 }, BaitOrder = new(){ 27585 } };
            if( fish.Fish.TryGetValue(28193, out f) )
                f.CatchData = new CatchData(5, 10){ BaitOrder = new(){ 28634 } };
            if( fish.Fish.TryGetValue(28066, out f) )
                f.CatchData = new CatchData(5, 10, 12, 19){ Folklore = 2507, CurrentWeather = new(){ 1 }, BaitOrder = new(){ 27590 } };
            if( fish.Fish.TryGetValue(28069, out f) )
                f.CatchData = new CatchData(5, 10, 0, 6){ Folklore = 2507, CurrentWeather = new(){ 11 }, BaitOrder = new(){ 27586, 27460 } };
            if( fish.Fish.TryGetValue(28192, out f) )
                f.CatchData = new CatchData(5, 10){ BaitOrder = new(){ 28634 } };
            if( fish.Fish.TryGetValue(28191, out f) )
                f.CatchData = new CatchData(5, 10){ Snagging = false, BaitOrder = new(){ 28634 } };
            if( fish.Fish.TryGetValue(28070, out f) )
                f.CatchData = new CatchData(5, 10, 20){ Folklore = 2507, CurrentWeather = new(){ 2, 1 }, BaitOrder = new(){ 27589 } };
            if( fish.Fish.TryGetValue(28071, out f) )
                f.CatchData = new CatchData(5, 10, 10, 12){ Folklore = 2507, BaitOrder = new(){ 27587, 27490, 27491 } };
            if( fish.Fish.TryGetValue(28719, out f) )
                f.CatchData = new CatchData(5, 10){ Snagging = true, Folklore = 2507, BaitOrder = new(){ 27590 } };
            if( fish.Fish.TryGetValue(28072, out f) )
                f.CatchData = new CatchData(5, 10, 6, 10){ Folklore = 2507, CurrentWeather = new(){ 3 }, BaitOrder = new(){ 27590 } };
            if( fish.Fish.TryGetValue(28925, out f) )
                f.CatchData = new CatchData(5, 20, 16){ Folklore = 2507, CurrentWeather = new(){ 3 }, BaitOrder = new(){ 27589 } };
            if( fish.Fish.TryGetValue(28926, out f) )
                f.CatchData = new CatchData(5, 20, 0, 2){ Folklore = 2507, BaitOrder = new(){ 27589 } };
            if( fish.Fish.TryGetValue(28927, out f) )
                f.CatchData = new CatchData(5, 20, 12, 16){ Folklore = 2507, CurrentWeather = new(){ 1 }, BaitOrder = new(){ 27584, 27461 } };
            if( fish.Fish.TryGetValue(28928, out f) )
                f.CatchData = new CatchData(5, 20){ Folklore = 2507, PreviousWeather = new(){ 2, 1 }, CurrentWeather = new(){ 4 }, BaitOrder = new(){ 27585 } };
            if( fish.Fish.TryGetValue(28929, out f) )
                f.CatchData = new CatchData(5, 20, 2, 12){ Folklore = 2507, PreviousWeather = new(){ 3, 4 }, CurrentWeather = new(){ 2 }, BaitOrder = new(){ 27587 } };
            if( fish.Fish.TryGetValue(28930, out f) )
                f.CatchData = new CatchData(5, 20, 12, 14){ Folklore = 2507, CurrentWeather = new(){ 2, 1 }, BaitOrder = new(){ 27590 } };
            if( fish.Fish.TryGetValue(30432, out f) )
                f.CatchData = new CatchData(5, 30, 22){ Folklore = 2507, CurrentWeather = new(){ 1, 2 }, BaitOrder = new(){ 27582 } };
            if( fish.Fish.TryGetValue(30433, out f) )
                f.CatchData = new CatchData(5, 30){ Folklore = 2507, PreviousWeather = new(){ 4 }, CurrentWeather = new(){ 1, 2 }, BaitOrder = new(){ 27589 } };
            if( fish.Fish.TryGetValue(30434, out f) )
                f.CatchData = new CatchData(5, 30){ Folklore = 2507, BaitOrder = new(){ 27589 } };
            if( fish.Fish.TryGetValue(30487, out f) )
                f.CatchData = new CatchData(5, 30){ Folklore = 2507, BaitOrder = new(){ 27590 } };
            if( fish.Fish.TryGetValue(30435, out f) )
                f.CatchData = new CatchData(5, 30, 6, 8){ Folklore = 2507, CurrentWeather = new(){ 7 }, BaitOrder = new(){ 27585 } };
            if( fish.Fish.TryGetValue(30593, out f) )
                f.CatchData = new CatchData(5, 30){ BaitOrder = new(){ 27587 } };
            if( fish.Fish.TryGetValue(30436, out f) )
                f.CatchData = new CatchData(5, 30){ Folklore = 2507, PreviousWeather = new(){ 1, 2 }, CurrentWeather = new(){ 1 }, BaitOrder = new(){ 27589 } };
            if( fish.Fish.TryGetValue(30437, out f) )
                f.CatchData = new CatchData(5, 30, 18){ Folklore = 2507, PreviousWeather = new(){ 1, 2 }, CurrentWeather = new(){ 4 }, BaitOrder = new(){ 27589 } };
            if( fish.Fish.TryGetValue(30438, out f) )
                f.CatchData = new CatchData(5, 30, 0, 3){ Folklore = 2507, PreviousWeather = new(){ 3 }, CurrentWeather = new(){ 1 }, BaitOrder = new(){ 27590 } };
            if( fish.Fish.TryGetValue(30439, out f) )
                f.CatchData = new CatchData(5, 30, 18, 20){ Folklore = 2507, CurrentWeather = new(){ 1 }, BaitOrder = new(){ 27590 } };
            if( fish.Fish.TryGetValue(32049, out f) )
                f.CatchData = new CatchData(5, 40, 6, 8){ Folklore = 2507, PreviousWeather = new(){ 1 }, CurrentWeather = new(){ 2 }, BaitOrder = new(){ 27589 } };
            if( fish.Fish.TryGetValue(32050, out f) )
                f.CatchData = new CatchData(5, 40){ Folklore = 2507, PreviousWeather = new(){ 3 }, CurrentWeather = new(){ 4 }, BaitOrder = new(){ 27585 } };
            if( fish.Fish.TryGetValue(32051, out f) )
                f.CatchData = new CatchData(5, 40, 22){ Folklore = 2507, CurrentWeather = new(){ 3 }, BaitOrder = new(){ 27588, 27457 } };
            if( fish.Fish.TryGetValue(32052, out f) )
                f.CatchData = new CatchData(5, 40, 12, 16){ Folklore = 2507, PreviousWeather = new(){ 2 }, CurrentWeather = new(){ 10 }, BaitOrder = new(){ 27585 } };
            if( fish.Fish.TryGetValue(32053, out f) )
                f.CatchData = new CatchData(5, 40, 16, 20){ Folklore = 2507, CurrentWeather = new(){ 4 }, BaitOrder = new(){ 27587, 27492 } };
            if( fish.Fish.TryGetValue(32054, out f) )
                f.CatchData = new CatchData(5, 40, 8, 11){ Folklore = 2507, PreviousWeather = new(){ 1 }, CurrentWeather = new(){ 3 }, BaitOrder = new(){ 27590 } };
        }
    }
}
﻿


namespace CS2PlayerSettings.Data.Repository.Models
{
    public class CrosshairCodeValue
    {
        public string CrosshairCode { get; set; }
        public double cl_crosshairsize { get; set; }
        public double cl_crosshairthickness { get; set; }
        public double cl_crosshairgap { get; set; }

        public double cl_fixedcrosshairgap { get; set; }
        public double cl_crosshair_outlinethickness { get; set; }

        public int cl_crosshair_dynamic_splitdis { get; set; }
        public double cl_crosshair_dynamic_maxdist_splitratio { get; set; }
        public double cl_crosshair_dynamic_splitalpha_innermod { get; set; }
        public double cl_crosshair_dynamic_splitalpha_outermod { get; set; }

        public int cl_crosshairusealpha { get; set; }
        public int cl_crosshair_drawoutline { get; set; }
        public int cl_crosshair_recoil { get; set; }
        public int cl_crosshairdot { get; set; }
        public int cl_crosshair_t { get; set; }
        public int cl_crosshairgap_useweaponvalue { get; set; }
        public int cl_crosshaircolor_r { get; set; }
        public int cl_crosshaircolor_g { get; set; }
        public int cl_crosshaircolor_b { get; set; }
        public int cl_crosshairalpha { get; set; }
        public int cl_crosshaircolor { get; set; }
        public int cl_crosshairstyle { get; set; }
    }
}

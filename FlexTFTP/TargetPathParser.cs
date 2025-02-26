using System.Collections.Generic;

namespace FlexTFTP
{
    public static class TargetPathParser
    {
        private static readonly Dictionary<string, string> Dict = new Dictionary<string, string>()
        {
            /* FlexDevice-M Binder */
            { "3-0028_application.s19", "cpu/application"       },
            { "3-0028_application.fpga", "fpga/application"     },

            /* FlexDevice-L 1. Generation */
            { "3-0087_preloader.s19", "cpu/preloader"           },
            { "3-0087_fallback.s19", "cpu/fallback"             },
            { "3-0087_distribution.s19", "cpu/distribution"     },
            { "3-0087_application.s19", "cpu/application"       },
            { "3-0087_fallback.fpga", "fpga/fallback"           },
            { "3-0087_distribution.fpga", "fpga/distribution"   },
            { "3-0087_application.fpga", "fpga/application"     },
            { "3-00870A01_preloader.s19", "cpu/preloader"           },
            { "3-00870A01_fallback.s19", "cpu/fallback"             },
            { "3-00870A01_distribution.s19", "cpu/distribution"     },
            { "3-00870A01_application.s19", "cpu/application"       },
            { "3-00870A01_fallback.fpga", "fpga/fallback"           },
            { "3-00870A01_distribution.fpga", "fpga/distribution"   },
            { "3-00870A01_application.fpga", "fpga/application"     },

            /* FlexDevice-L 2. Generation */
            { "3-00870A02_preloader.s19", "cpu/preloader"           },
            { "3-00870A02_fallback.s19", "cpu/fallback"             },
            { "3-00870A02_distribution.s19", "cpu/distribution"     },
            { "3-00870A02_application.s19", "cpu/application"       },
            { "3-00870A02_fallback.fpga", "fpga/fallback"           },
            { "3-00870A02_distribution.fpga", "fpga/distribution"   },
            { "3-00870A02_application.fpga", "fpga/application"     },

            /* FlexDevice-L 3. Generation */
            { "3-00870A03_preloader.s19", "cpu/preloader"           },
            { "3-00870A03_fallback.s19", "cpu/fallback"             },
            { "3-00870A03_distribution.s19", "cpu/distribution"     },
            { "3-00870A03_application.s19", "cpu/application"       },
            { "3-00870A03_fallback.fpga", "fpga/fallback"           },
            { "3-00870A03_distribution.fpga", "fpga/distribution"   },
            { "3-00870A03_application.fpga", "fpga/application"     },

            /* FlexDevice-L² 1. Generation */
            { "3-00870S01_preloader.s19", "cpu/preloader"           },
            { "3-00870S01_fallback.s19", "cpu/fallback"             },
            { "3-00870S01_distribution.s19", "cpu/distribution"     },
            { "3-00870S01_application.s19", "cpu/application"       },
            { "3-00870S01_fallback.fpga", "fpga/fallback"           },
            { "3-00870S01_distribution.fpga", "fpga/distribution"   },
            { "3-00870S01_application.fpga", "fpga/application"     },
            { "3-00870S01_fallback.fpga2", "fpga2/fallback"         },
            { "3-00870S01_distribution.fpga2", "fpga2/distribution" },
            { "3-00870S01_application.fpga2", "fpga2/application"   },

            /* FlexDevice-L² 2. Generation */
            { "3-00870S02_preloader.s19", "cpu/preloader"           },
            { "3-00870S02_fallback.s19", "cpu/fallback"             },
            { "3-00870S02_distribution.s19", "cpu/distribution"     },
            { "3-00870S02_application.s19", "cpu/application"       },
            { "3-00870S02_fallback.fpga", "fpga/fallback"           },
            { "3-00870S02_distribution.fpga", "fpga/distribution"   },
            { "3-00870S02_application.fpga", "fpga/application"     },
            { "3-00870S02_fallback.fpga2", "fpga2/fallback"         },
            { "3-00870S02_distribution.fpga2", "fpga2/distribution" },
            { "3-00870S02_application.fpga2", "fpga2/application"   },

            /* FlexDevice-L² 3. Generation */
            { "3-00870S03_preloader.s19", "cpu/preloader"           },
            { "3-00870S03_fallback.s19", "cpu/fallback"             },
            { "3-00870S03_distribution.s19", "cpu/distribution"     },
            { "3-00870S03_application.s19", "cpu/application"       },
            { "3-00870S03_fallback.fpga", "fpga/fallback"           },
            { "3-00870S03_distribution.fpga", "fpga/distribution"   },
            { "3-00870S03_application.fpga", "fpga/application"     },
            { "3-00870S03_fallback.fpga2", "fpga2/fallback"         },
            { "3-00870S03_distribution.fpga2", "fpga2/distribution" },
            { "3-00870S03_application.fpga2", "fpga2/application"   },

            /* FlexDevice-L² 2. Generation Second SoC */
            { "3-00870S02_2_preloader.s19", "cpu/preloader"         },
            { "3-00870S02_2_application.s19", "cpu/application"     },

            /* FlexDevice-L² 3. Generation Second SoC */
            { "3-00870S03_2_preloader.s19", "cpu/preloader"         },
            { "3-00870S03_2_application.s19", "cpu/application"     },

            /* FlexDevice-L² HDSub AS 1. Generation */
            { "3-00870N01_preloader.s19", "cpu/preloader"           },
            { "3-00870N01_fallback.s19", "cpu/fallback"             },
            { "3-00870N01_distribution.s19", "cpu/distribution"     },
            { "3-00870N01_application.s19", "cpu/application"       },
            { "3-00870N01_fallback.fpga", "fpga/fallback"           },
            { "3-00870N01_distribution.fpga", "fpga/distribution"   },
            { "3-00870N01_application.fpga", "fpga/application"     },
            { "3-00870N01_fallback.fpga2", "fpga2/fallback"         },
            { "3-00870N01_distribution.fpga2", "fpga2/distribution" },
            { "3-00870N01_application.fpga2", "fpga2/application"   },

            /* FlexDevice-L² HDSub AS 2. Generation */
            { "3-00870N02_preloader.s19", "cpu/preloader"           },
            { "3-00870N02_fallback.s19", "cpu/fallback"             },
            { "3-00870N02_distribution.s19", "cpu/distribution"     },
            { "3-00870N02_application.s19", "cpu/application"       },
            { "3-00870N02_fallback.fpga", "fpga/fallback"           },
            { "3-00870N02_distribution.fpga", "fpga/distribution"   },
            { "3-00870N02_application.fpga", "fpga/application"     },
            { "3-00870N02_fallback.fpga2", "fpga2/fallback"         },
            { "3-00870N02_distribution.fpga2", "fpga2/distribution" },
            { "3-00870N02_application.fpga2", "fpga2/application"   },

            /* FlexDevice-S */
            { "3-00860A01_preloader.s19", "cpu/preloader"           },
            { "3-00860A01_fallback.s19", "cpu/fallback"             },
            { "3-00860A01_distribution.s19", "cpu/distribution"     },
            { "3-00860A01_application.s19", "cpu/application"       },
            { "3-00860A01_fallback.fpga", "fpga/fallback"           },
            { "3-00860A01_distribution.fpga", "fpga/distribution"   },
            { "3-00860A01_application.fpga", "fpga/application"     },
            { "3-00860A01_fallback.fpga2", "fpga2/fallback"         },
            { "3-00860A01_distribution.fpga2", "fpga2/distribution" },
            { "3-00860A01_application.fpga2", "fpga2/application"   },

            /* FlexCard-PXIe3 */
            { "3-00940A01_preloader.s19", "cpu/preloader"           },
            { "3-00940A01_fallback.s19", "cpu/fallback"             },
            { "3-00940A01_distribution.s19", "cpu/distribution"     },
            { "3-00940A01_application.s19", "cpu/application"       },
            { "3-00940A01_fallback.fpga", "fpga/fallback"           },
            { "3-00940A01_distribution.fpga", "fpga/distribution"   },
            { "3-00940A01_application.fpga", "fpga/application"     },
            { "3-00940A01_fallback.fpga2", "fpga2/fallback"         },
            { "3-00940A01_distribution.fpga2", "fpga2/distribution" },
            { "3-00940A01_application.fpga2", "fpga2/application"   },

            /* FlexCard-PCIe3 */
            { "3-00950A01_preloader.s19", "cpu/preloader"           },
            { "3-00950A01_fallback.s19", "cpu/fallback"             },
            { "3-00950A01_distribution.s19", "cpu/distribution"     },
            { "3-00950A01_application.s19", "cpu/application"       },
            { "3-00950A01_fallback.fpga", "fpga/fallback"           },
            { "3-00950A01_distribution.fpga", "fpga/distribution"   },
            { "3-00950A01_application.fpga", "fpga/application"     },
            { "3-00950A01_fallback.fpga2", "fpga2/fallback"         },
            { "3-00950A01_distribution.fpga2", "fpga2/distribution" },
            { "3-00950A01_application.fpga2", "fpga2/application"   },


            /* FlexInterface-S */
            { "3-00862A01_distribution.s19", "cpu/distribution"     },

            /* FlexInterface-L */
            { "3-00872A01_distribution.s19", "cpu/distribution"     },

            /* FlexInterface-PXIe */
            { "3-00942A01_distribution.s19", "cpu/distribution"     },

            /* FlexInterface-PCIe */
            { "3-00952A01_distribution.s19", "cpu/distribution"     },

            /* WIFI Application (ESP32 Extension Board */
            { "3-00870V01_application.s19", "wifi/application"      },

            /* FlexSystem-XS */
            { "3-01040A01_application.s19", "cpu/application"           },

            /* FlexSystem-XS Squared */
            { "3-01040C01_application.s19", "cpu/application"           },

            /* FlexSystem-XS Cubed */
            { "3-01040D01_application.s19", "cpu/application"           },

            /* FlexSystem-M */
            { "3-01050A01_application.s19", "cpu/application"           },
        };

        public static string GetPathByName(string name)
        {
            foreach (KeyValuePair<string, string> entry in Dict)
            {
                if (name.EndsWith(entry.Key))
                {
                    return entry.Value;
                }
            }
            return null;
        }

        public static string GetTargetPath(string file)
        {
            try
            {
                SRecord srecord = new SRecord(file);
                string targetName = srecord.GetHeaderEntry();

                string targetPath = GetPathByName(targetName);
                if (targetPath != null)
                {
                    return targetPath; 
                }
            }
            catch
            {
                return null;
            }
            return null;
        }

        public static List<string> GetAllPaths()
        {
            List<string> allPaths = new List<string>();

            foreach (KeyValuePair<string, string> entry in Dict)
            {
                allPaths.Add(entry.Value);
            }

            return allPaths;
        }
    }
}

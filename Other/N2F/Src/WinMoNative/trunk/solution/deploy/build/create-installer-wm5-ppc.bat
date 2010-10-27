@echo off

echo "Make n2f-sup (debug)"

cabwiz.exe  n2f_sup_wm5ppc_debug.inf /err err_n2f_sup_wm5ppc_log_debug.err


echo "Make n2f-sup (release)"

cabwiz.exe  n2f_sup_wm5ppc_release.inf /err err_n2f_sup_wm5ppc_log_release.err

echo "Done"




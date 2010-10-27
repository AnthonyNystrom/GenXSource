@echo off

echo "Make n2f-sup (debug)"

cabwiz.exe  n2f_sup_wm6pro_debug.inf /err err_n2f_sup_wm6pro_log_debug.err


echo "Make n2f-sup (release)"

cabwiz.exe  n2f_sup_wm6pro_release.inf /err err_n2f_sup_wm6pro_log_release.err

echo "Done"




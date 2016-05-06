function samples_window_sysMin_BtnClick(btn, wparam, lparam)
{
	winManager.ShowWindow(btn.hwnd, 6); //SW_MINIMIZE
}

function samples_window_sysMax_CkClick(btn, wparam, lparam)
{
	if (wparam)
		winManager.ShowWindow(btn.hwnd, 3); //SW_MAXIMIZE
	else
		winManager.ShowWindow(btn.hwnd, 9); //SW_RESTORE
}

function samples_window_sysClose_BtnClick(btn, wparam, lparam)
{
	winManager.DestroyWindow(btn.hwnd);
}

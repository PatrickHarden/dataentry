# From https://github.com/pyinstaller/pyinstaller/issues/4400#issuecomment-560099108
# Fixes ImportError: cannot import name 'pywrap_tensorflow' from
# 'tensorflow_core.python' when running a pyinstaller built executable
# 'tensorflow==2.0.0'

from PyInstaller.utils.hooks import collect_submodules, collect_data_files

hiddenimports = collect_submodules('tensorflow_core')
hiddenimports += collect_submodules('sklearn')

datas = collect_data_files('tensorflow_core', subdir=None, include_py_files=False)
datas += collect_data_files('astor', subdir=None, include_py_files=False)
datas += collect_data_files('sklearn', subdir=None, include_py_files=False)



prerequisites:
python should be installed as well as pip in order to install pyinstaller with command below and be able to generated exe and and dlls for running watermark detection
`pip install pyinstaller`

how to build:
from within WatermarkDetection folder:
`pyinstaller classify_Single_Batch.py --onedir --additional-hooks-dir=hooks`

how to run (test):
`.\dist\classify_Single_Batch\classify_Single_Batch.exe -m wmb2.model -l lblb2.pickle -i image.jpg`

running as python script
`py classify_Single_Batch.py -m wmb2.model -l lblb2.pickle -i image.jpg`

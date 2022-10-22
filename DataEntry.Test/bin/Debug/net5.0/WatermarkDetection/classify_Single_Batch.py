#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Created on Wed May 30 09:43:35 2018

@author: KSrivasta
"""
import json
import keras
import numpy as np
import argparse
import imutils
import pickle
import random
import cv2
import os
import pandas as pd
from keras.models import load_model
from keras.preprocessing.image import ImageDataGenerator
from keras.preprocessing.image import img_to_array
from sklearn.preprocessing import LabelBinarizer
import matplotlib.pyplot as plt
from imutils import paths

import warnings
warnings.filterwarnings("ignore")

# construct the argument parse and parse the arguments
ap = argparse.ArgumentParser()
ap.add_argument("-m", "--model", required=True,
    help="path to trained model model")
ap.add_argument("-l", "--labelbin", required=True,
    help="path to label binarizer")
ap.add_argument("--p", action='store_true', default=False,
    help="print log")
ap.add_argument("--c", action='store_true', default=False,
    help="generate spreadsheet")
ap.add_argument("-d", "--dataset", required=False,
    help="path to input dataset (i.e., directory of images)")
ap.add_argument("-i", "--image", required=False,
    help="path to input image")
args = vars(ap.parse_args())

# grab the image paths
if (args["p"]):
    print("[INFO] loading images...")

if (args["image"] != None):
    img = args["image"]
    image =cv2.imread(args["image"])
    imagePaths = None
else:
    image = None
    imagePaths = sorted(list(paths.list_images(args["dataset"])))

data = []

if (imagePaths != None):
    # load the trained convolutional neural network and the label binarizer
    if (args["p"]):
        print("[INFO] loading network...")
    buildJson = { 'prediction' : []}
    model = load_model(args["model"])
    lb = pickle.loads(open(args["labelbin"], "rb").read())
    for imagePath in imagePaths:
        # load the image
        row = []
        image = cv2.imread(imagePath)
        if (args["p"]):
            print(imagePath)
        output = image.copy()

        # pre-process the image for classification
        image = cv2.resize(image, (224, 224))
        image = image.astype("float") / 255.0
        image = img_to_array(image)
        image = np.expand_dims(image, axis=0)

        # classify the input image
        if (args["p"]):
            print("[INFO] classifying image...")

        proba = model.predict(image)[0]
        idx = np.argmax(proba)
        label = lb.classes_[idx]
        buildJson["prediction"].append({'label':label, 'idx': int(idx), 'confidence': proba.tolist() }) 
        if (args["c"]):
            row.append(imagePath)
            row.append(label)
            row.append(idx)
            row.append(proba)
            data.append(row)

    jsonResult = json.dumps(buildJson)    
    if (args["c"]):
        # create dataframe to store the result
        dataF = pd.DataFrame(data)
        dataF.columns = ['ImagePath', 'Prediction', 'Index', 'Confidence']
        dataF.to_csv('Predictions.csv', index=False)
    
else:
    if (args["p"]):
        print("Single Image Watermark Detection")
    output = image.copy()
    row = []
    image = cv2.resize(image, (224, 224))
    image = image.astype("float") / 255.0
    image = img_to_array(image)
    image = np.expand_dims(image, axis=0)

    # load the trained convolutional neural network and the label binarizer
    model = load_model(args["model"])
    lb = pickle.loads(open(args["labelbin"], "rb").read())

    # classify the input image
    if (args["p"]):
        print("[INFO] classifying image...")
    proba = model.predict(image)[0]
    idx = np.argmax(proba)
    label = lb.classes_[idx]

    buildJson = { 'prediction' : []}
    buildJson["prediction"].append({'label':label, 'idx': int(idx), 'confidence': proba.tolist() }) 
    jsonResult = json.dumps(buildJson)

    if (args["c"]):
        row.append(img)
        row.append(label)
        row.append(idx)
        row.append(proba)
        data.append(row)

    
    
    # build 
    if (args["c"]):
        # create dataframe to store the result
        dataF = pd.DataFrame(data)
        dataF.columns = ['ImagePath', 'Prediction', 'Index', 'Confidence']
        dataF.to_csv('Predictions.csv', index=False)

print(jsonResult)


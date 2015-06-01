ICA-SVP Server
========

<img src="Images/ICA_SVP_logo.png?raw=true" height="100"/>

WHAT IS ICA-SVP?
--------------

The influence of cognitive load, environmental factors and emotional arousal on pupillary responses has been widely studied in the psychophysiology literature. Most experiments in such studies made use of specialized clinical equipment to collect eye data, which can be expensive and therefore not accessible to anyone willing to conduct similar experiments. Consequently, many efforts have been made to use video-based eye tracking systems as pupillometers. ICA-SVP is a software tool for such purpose that can be used out of the box for real-time cognitive effort estimation and pupillary response classification based on visual stimuli and eye-tracking. The cognitive effort is quantified according to the [index of cognitive activity (ICA)] (https://encrypted.google.com/patents/US6090051). Serial visual presentation (SVP) is used to present discrete tasks at certain configurable speed.

WHAT CAN I DO WITH IT?
---------

ICA-SVP allows you to conduct experiments on which cognitive effort and pupillary response induced by visual stimuli can be evaluated. Experiment participants are placed in front of a visual display and their head is fixed (e.g. using a pillow). Next, a stimuli sequence is presented serially while a remote eye tracker collects data from the eyes of the subject. In order to reduce the influence of head movement on pupil size measurements, when the participant loses the focus from the visual item (determined by the gaze position), the presentation automatically pauses. Moreover, the presentation resumes when the participant focus back into it.

<img src="Images/Experiment_setup.png?raw=true" height="200"/>

HOW TO USE?
---------

- Download the source code and open the solution in a compliant [Visual Studio 12](https://www.visualstudio.com/) version.
- Set the TCP port on which your SVP client will connect. Go to ICA_SVP.Misc.Config and modify the NET_SERVER_PORT parameter.
- Compile and run it.
- Install, configure and run the [ICA-SVP web client] (https://github.com/centosGit/RSVP-Client).

HOW TO CONNECT AN SVP CLIENT?
---------

The ICA-SVP web client will automatically connect to the server when started.

LOGS
---------

Although the cognitive effort is estimated and categorized in real-time, raw and processed data is stored in log files to allow future analysis and visualization. Default location /logs.

SOFTWARE ARCHITECTURE
--------------

The processing server holds the core functionality of the system. In this component, data from the eye tracker and the SVP client are collected, cleaned and transformed to be later used to produce an estimation of cognitive effort and a categorization of the incoming discrete signal. The server is implemented based on the pipes and filters design pattern, where the whole algorithm is decomposed into several steps and modeled as a set of connected filters.

<img src="Images/ICA-SVP_Software_Architecture.png?raw=true" height="300"/>

SOFTWARE DEPENDENCIES
---------
[ICA-SVP web client] (https://github.com/centosGit/RSVP-Client) </br>
[.NET Framework 4.5] (https://www.microsoft.com/en-us/download/details.aspx?id=30653) <br/>
[The EyeTribe SDK 0.9.49] (https://theeyetribe.com/) <br/>
[Weka 3.6.12] (http://www.cs.waikato.ac.nz/) </br>
[MATLAB Wavelet Toolbox 8.5.0.197613 (R2015a)] (http://se.mathworks.com/products/wavelet/) </br>
[Fleck] (https://github.com/statianzo/Fleck) <br/>

HARDWARE
--------------

[The EyeTribe] (https://theeyetribe.com/) eye tracker <br/>

<img src="Images/ICA-SVP_Hardware_Architecture.png?raw=true" height="300"/>

Collaborators:
--------------

[Daniel Garcia] (https://github.com/danielgg88) <dgac@itu.dk>, [Ioannis Sintos] (https://github.com/centosGit) <isin@itu.dk>, John Paulin Hansen <paulin@itu.dk>

[IT University of Copenhagen](http://www.itu.dk/en)

DISCLAIMER
--------------

The software is an experimental tool built for research and educational purposes. The reliability of the of the results cannot be guaranteed. 

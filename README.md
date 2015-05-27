ICA-SVP Server: Classifying task-evoked pupillary response and indexing cognitive load provoked by visual stimuli
========

<img src="Images/ICA_SVP_logo.png?raw=true" height="150" width="150"/>

WHAT IS ICA-SVP?
--------------

The influence of cognitive load, environmental factors and emotional arousal on pupillary responses has been widely studied in the psychophysiology literature. Most experiments in such studies made use of specialized clinical equipment to collect eye data, which can be expensive and therefore not accessible to anyone willing to conduct similar experiments. Consequently, many efforts have been made to use video-based eye tracking systems as pupillometers. However, no software tool for such purposes that can be used out of the box has been developed yet. 

ICA-SVP is a software tool for real-time cognitive effort estimation and pupillary response classification based on visual stimuli and eye-tracking. The cognitive effort is quantified according to the [index of cognitive activity (ICA)] (https://encrypted.google.com/patents/US6090051). Serial visual presentation (SVP) is used to present discrete tasks at certain configurable speed.

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

HOW TO USE?
---------

- Download the source code and open the solution in a compliant [Visual Studio 12](https://www.visualstudio.com/) version.
- Set the TCP port on which your SVP client will connect. Go to ICA_SVP.Misc.Config and modify the NET_SERVER_PORT parameter.
- Compile and run it!

HOW TO CONNECT AN SVP CLIENT
---------

The ICA-SVP web client will automatically connect to the server when started.

Collaborators:
--------------
Daniel Garcia <dgac@itu.dk>, Ioannis Sintos <isin@itu.dk>, John Paulin Hansen <paulin@itu.dk>

[IT University of Copenhagen](http://www.itu.dk/en)

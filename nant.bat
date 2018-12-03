@echo off

REM tools\nant-0.86-beta1\bin\nant.exe %1

REM
REM we are using this nightly build because it supports
REM using assembly references when unit testing
REM
tools\nant-0.86-nightly-2008-06-02\bin\nant.exe %1
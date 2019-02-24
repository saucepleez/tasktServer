import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import TasktMetricCard from './components/TasktMetricCard';
import './BotDashboard.css'
import TaskList from './components/TaskList';
import TopWorker from './components/TopWorkers';
import Select from 'react-select';
import Modal from 'react-responsive-modal';
import AddTask from './components/AddTask';
import Loader from '../Loader';
export class BotDashboard extends Component {
    displayName = BotDashboard.name
    timelineScope = 1440
    startDate
    constructor(props) {
        super(props);

        var date = new Date();
        var formattedDate = (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear();

        this.state = {
            timelineScope: 200,
            startDate: formattedDate,
        }
    }
   
        render()
        {
            console.log('calculated date: ' + this.state.startDate);
        return (
            <div>
                <h1>My Dashboard</h1>
                
                <p>View statistics and information about your workers.  Tasks are automatically closed if a worker does not update when finished and attempts to launch another task.</p>

                <TasktMetricCard metricName="Tasks Running Now" api="/api/Tasks/Metrics/Running" startDate={this.state.startDate}></TasktMetricCard>
                <TasktMetricCard metricName="Tasks Completed" api="/api/Tasks/Metrics/Completed" startDate={this.state.startDate}></TasktMetricCard>
                <TasktMetricCard metricName="Tasks Closed" api="/api/Tasks/Metrics/Closed" startDate={this.state.startDate}></TasktMetricCard>
                <TasktMetricCard metricName="Tasks Errored" api="/api/Tasks/Metrics/Errored" startDate={this.state.startDate}></TasktMetricCard>

                <h2>Top Workers</h2>
                <p>View your top workers for the last 24 hours.</p>
                <TopWorker api="/api/Workers/Top"></TopWorker>

                <h2>Latest Tasks</h2>
                <AddTask></AddTask>
                <p>Live view of the latest tasks being executed by your workers.</p>
                <TaskList api="/api/Tasks/All"></TaskList>
       
            </div>
            );

        }
  }




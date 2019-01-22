import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import Button from '@material-ui/core/Button';
import WorkerList from './components/WorkerList';

export class WorkForce extends Component {
    displayName = WorkForce.name

    render() {
        return (

            <div>
                <h1>My Workforce</h1>         
                <WorkerList api="/api/Workers/All"></WorkerList>
            </div>
        );
    }
}



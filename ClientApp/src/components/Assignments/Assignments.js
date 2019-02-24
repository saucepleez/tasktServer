import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import Button from '@material-ui/core/Button';
import AssignmentList from './components/AssignmentList';
import AddAssignment from './components/AddAssignment'

export class Assignments extends Component {
    displayName = Assignments.name

    render() {
        return (

            <div>
                <h1>My Assignments</h1>     
                <AddAssignment></AddAssignment>
                <p>Assignments are scheduled tasks that can either be executed by a single worker or a member of a specific worker pool.</p>
                <AssignmentList api="/api/Assignments/All"></AssignmentList>
            </div>
        );
    }
}



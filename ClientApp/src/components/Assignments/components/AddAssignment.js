import React from 'react';
import PropTypes from 'prop-types';
import Modal from 'react-responsive-modal';
import { Button, MenuItem, FormControl, FormGroup, ControlLabel, Glyphicon } from 'react-bootstrap';
export default class AddAssignment extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            newTaskModalOpen: false,
            executableTasks: [],
            workers: [],
            isLoaded: false,
            selectedScript: '',
            selectedWorker: '',
            workerLastCheckIn: '',
            assignmentEnabled: '',
            assignmentFrequency: 5,
            assignmentInterval: 0,

        }

        this.handleSubmit = this.handleSubmit.bind(this);
        this.updateSelectedScript = this.updateSelectedScript.bind(this);
        this.updateSelectedWorker = this.updateSelectedWorker.bind(this);
        this.updateAssignmentName = this.updateAssignmentName.bind(this);
        this.updateAssignmentEnabled = this.updateAssignmentEnabled.bind(this);
        this.updateAssignmentFrequency = this.updateAssignmentFrequency.bind(this);
        this.updateAssignmentInterval = this.updateAssignmentInterval.bind(this);

    }

    updateAssignmentName() {

    }
    updateAssignmentEnabled() {

    }
    updateAssignmentFrequency(event) {

        let frequencyString = event.target.value;
        let parsedFrequency = parseInt(frequencyString);

        this.setState({ assignmentInterval: parsedFrequency });
    }
    updateAssignmentInterval(cbo) {
        this.setState({ assignmentInterval: cbo.target.selectedIndex });
    }
    updateSelectedScript(cbo) {

        if (cbo.target.value != 'Please Select a Script') {
            console.log('User Selected Script Value ', cbo.target.value)
            this.setState({ selectedScript: cbo.target.value });
        }
        else {
            this.setState({ selectedScript: '' });
        }


    }
    updateSelectedWorker(cbo) {

        if (cbo.target.value != 'Please Select a Worker') {
            console.log('User Selected Worker Value ', cbo.target.value)
            var workerData = this.findArrayElementByWorkerID(this.state.workers, cbo.target.value);
            this.setState({ selectedWorker: cbo.target.value, workerLastCheckIn: 'Last Seen @ ' + workerData.lastCheckIn });
        }
        else {
            this.setState({ selectedWorker: '', workerLastCheckIn: '' });
        }

    }

    findArrayElementByWorkerID(array, workerID) {
        console.log("searching array ", array);
        return array.find((element) => {
            return element.workerID === workerID;
        })
    }

    showModal = () => {
        console.log("User requested Show Modal");

        this.setState({
            newTaskModalOpen: true
        });

        this.LoadData();

    }

    hideModal = () => {
        console.log("User requested Hide Modal");
        this.setState({
            newTaskModalOpen: false
        });
    }


    handleSubmit(event) {

        console.log('User Submitted Assignment  ', this.state.selectedScript, ' and Worker ', this.state.selectedWorker);


        if (this.state.selectedScript == '' || this.state.selectedWorker == '') {
            event.preventDefault();
            return;
        }


        this.setState({
            newTaskModalOpen: false,
        });

        fetch('/api/Assignments/Add', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                assignmentName: this.state.assignmentName,
                frequency: this.state.assignmentFrequency,
                interval: this.state.assignmentInterval,
                enabled: this.state.assignmentEnabled,
                assignedWorker: this.state.selectedWorker,
                publishedScriptID: this.state.selectedScript,
            })
        },
            (error) => {
                alert(error);
                console.log(error);
            })



        event.preventDefault();
    }



    LoadData() {
        console.log("Loading Data from /api/Scripts/All");


        //fetch all scripts
        fetch('/api/Scripts/All')
            .then(res => res.json())
            .then(
                (result) => {
                    console.log("Data Loaded from /api/Scripts/All: ", JSON.stringify(result));


                    this.setState({
                        executableTasks: result
                    });

                },
                (error) => {
                    alert(error);
                    console.log(error);
                }
            )

        console.log("Loading Data from /api/Workers/All");

        //fetch all workers
        fetch('/api/Workers/All')
            .then(res => res.json())
            .then(
                (result) => {
                    console.log("Data Loaded from /api/Scripts/All: ", JSON.stringify(result));


                    this.setState({
                        workers: result
                    });

                },
                (error) => {
                    alert(error);
                    console.log(error);
                }
            )

        this.setState({
            isLoaded: true,
        });

    }

    render() {
        const { newTaskModalOpen, executableTasks, isLoaded, workers, workerLastCheckIn } = this.state;

        console.log("Rendering Add Task Modal: ", this.state.newTaskModalOpen, this.state.isLoaded);

        const pStyle = {
            color: 'steelblue',
        };

        if (!this.state.newTaskModalOpen) {
            return <Button onClick={this.showModal}>New Assignment</Button>;
        }
        else if (this.state.isLoaded) {
            return (

                <div>
                    <Button onClick={this.showModal}>New Assignment</Button>
                    <Modal open={this.state.newTaskModalOpen} onClose={this.hideModal} center>
                        <form onSubmit={this.handleSubmit}>
                            <h1 style={pStyle}><Glyphicon glyph='plus' />Add Assignment</h1>

                            <FormGroup controlId="assignmentName">
                                <ControlLabel><Glyphicon glyph='user' />Enter Assignment Name</ControlLabel>
                                <FormControl type="text" placeholder="My Assignment" />
                            </FormGroup>


                            <FormGroup controlId="scriptSelect">
                                <ControlLabel><Glyphicon glyph='file' />Select a Script</ControlLabel>
                                <FormControl componentClass="select" placeholder="Select a Script" onChange={this.updateSelectedScript}>
                                    <option key="0" value="Please Select a Script">Please Select a Script</option>
                                    {executableTasks.map(task =>
                                        <option key={task.publishedScriptID} value={task.publishedScriptID}>{task.friendlyName} ({task.publishedOn})</option>
                                    )};
                            </FormControl>
                            </FormGroup>


                            <FormGroup controlId="workerSelect">
                                <ControlLabel><Glyphicon glyph='user' />Select a Worker</ControlLabel>
                                <FormControl componentClass="select" placeholder="Select a Worker" onChange={this.updateSelectedWorker}>
                                    <option key="0" value="Please Select a Worker">Please Select a Worker</option>
                                    {workers.map(task =>
                                        <option key={task.workerID} value={task.workerID}>{task.userName}@{task.machineName} [{task.workerID}]</option>
                                    )};
                                </FormControl>
                                <label>{this.state.workerLastCheckIn}</label>
                            </FormGroup>

                            <FormGroup controlId="frequencySelect">
                                <ControlLabel><Glyphicon glyph='user' />Enter Assignment Frequency</ControlLabel>
                                <FormControl type="text" placeholder="5" onChange={this.updateAssignmentFrequency.bind(this)} />
                            </FormGroup>

                            <FormGroup controlId="intervalSelect">
                                <ControlLabel><Glyphicon glyph='user' />Select Recurring Interval</ControlLabel>
                                <FormControl componentClass="select" placeholder="Select Recurring Interval" onChange={this.updateAssignmentInterval}>
                                    <option key="0" value="Seconds">Seconds</option>
                                    <option key="1" value="Minutes">Minutes</option>
                                    <option key="2" value="Days">Days</option>
                                    <option key="3" value="Months">Months</option>
                                </FormControl>
                            </FormGroup>

                            <Button type="submit">Submit</Button>
                        </form>
                    </Modal>
                </div>

            );
        }
        else {
            return (<Modal open={this.state.newTaskModalOpen} onClose={this.hideModal} center>
                Loading...
            </Modal>);
        }




    }



}


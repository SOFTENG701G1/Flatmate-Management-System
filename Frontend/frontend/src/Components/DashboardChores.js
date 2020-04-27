import React, { Component } from 'react';
import APIRequest from "../Util/APIRequest";
import CheckBoxListComponent from './CheckBoxListComponent';

export default class DashboardChores extends Component {
    constructor() {
        super();

        this.state = {
            chores: []
        };
    }

    async getAllChores() {
        const choreResult = await APIRequest.getChoresForFlat();
        return choreResult.json();
    }

    async updateChore(id) {
        await APIRequest.markChoreComplete(id)
    }

    async componentDidMount() {
        //Obtain chores associated with the logged in user Flat.
        await this.getAllChores().then(data => {
            if(!data) data = [];
            this.setState({
                chores: data
            });
        });
    }

    render () {
        const chores = this.state.chores;
        const choresDesc = [];
        const isChecked = [];
        const relatedId = [];
        
        for (let i = 0; i < chores.length; i++){
            choresDesc.push(chores[i]["description"]);
            isChecked.push(chores[i]["completed"]);
            relatedId.push(chores[i]["id"]);
        }

        return (
            <div>
                <CheckBoxListComponent
                    title="Chores"
                    listItems={choresDesc}
                    isChecked={isChecked}
                    relatedId={relatedId}
                    onListItemChanged={this.updateChore}
                />
            </div>
        );
    }
}
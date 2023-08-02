// Copyright (c) 2023, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.ComponentModel;

namespace Moryx.Workplans
{
    /// <summary>
    /// Default implementation of IWorkplan
    /// </summary>
    [DataContract]
    public class Workplan : IWorkplan, IPersistentObject
    {
        /// <summary>
        /// Create a new workplan instance
        /// </summary>
        public Workplan() : this(new List<IConnector>(), new List<IWorkplanStep>())
        {
        }

        /// <summary>
        /// Private constructor used for new and restored workplans
        /// </summary>
        private Workplan(List<IConnector> connectors, List<IWorkplanStep> steps)
        {
            _connectors = connectors;
            _steps = steps;
        }

        /// <see cref="IWorkplan"/>
        public long Id { get; set; }

        ///<see cref="IWorkplan"/>
        public string Name { get; set; }

        ///<see cref="IWorkplan"/>
        public int Version { get; set; }

        ///<see cref="IWorkplan"/>
        public WorkplanState State { get; set; }

        /// <summary>
        /// Current biggest id in the workplan
        /// </summary>
        public int MaxElementId { get; set; }

        /// <summary>
        /// Editable list of connectors
        /// </summary>
        [DataMember]
        private List<IConnector> _connectors;

        /// <see cref="IWorkplan"/>
        public IEnumerable<IConnector> Connectors => _connectors;

        /// <summary>
        /// Editable list of steps
        /// </summary>
        [DataMember]
        private List<IWorkplanStep> _steps;

        /// <see cref="IWorkplan"/>
        public IEnumerable<IWorkplanStep> Steps => _steps;

        /// <summary>
        /// Add a range of connectors to the workplan
        /// </summary>
        public void Add(params IWorkplanNode[] nodes)
        {
            foreach (var node in nodes)
            {
                node.Id = ++MaxElementId;
                if (node is IConnector)
                    _connectors.Add((IConnector)node);
                else
                    _steps.Add((IWorkplanStep)node);
            }
        }
        /// <summary>
        /// Removes a node from the workplan
        /// </summary>
        public bool Remove(IWorkplanNode node)
        {
            return node is IConnector ? _connectors.Remove((IConnector)node) : _steps.Remove((IWorkplanStep)node);
        }

        /// <summary>
        /// Restore a workplan with a list of connectors and steps
        /// </summary>
        public static Workplan Restore(List<IConnector> connectors, List<IWorkplanStep> steps)
        {
            return new Workplan(connectors, steps);
        }

        /// <summary>
        /// Compare two workplans
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            ///check whether the object corresponds to a Workplan
            if (!(obj is Workplan))
            {
                return false;
            }
            Workplan newPlan = (Workplan)obj;

        
            ///the connectors 'star', 'end' and 'failed are identified
            var start = this.Connectors.First(x => x.Name.Equals("Start"));
            var end = this.Connectors.First(x => x.Name.Equals("End"));
            var failed = this.Connectors.First(x => x.Name.Equals("Failed"));

            var newStart = newPlan.Connectors.First(x => x.Name.Equals("Start"));
            var newEnd = newPlan.Connectors.First(x => x.Name.Equals("End"));
            var newFailed = newPlan.Connectors.First(x => x.Name.Equals("Failed"));

            ///the first step is identified
            var step = this.Steps.First(x => x.Inputs.Any(y => y.Equals(start)));
            var newStep = newPlan.Steps.First(x => x.Inputs.Any(y => y.Equals(newStart)));

            ///lists to note which steps are still need to be checked
            List<IWorkplanStep> note = new List<IWorkplanStep>();
            List<IWorkplanStep> newNote = new List<IWorkplanStep>();

            ///lists to note the steps already checked
            List<IWorkplanStep> check = new List<IWorkplanStep>();
            List<IWorkplanStep> newCheck = new List<IWorkplanStep>();
            
            ///the first step is added to the note list 
            note.Add(step);
            newNote.Add(newStep); 
           
            ///condition: there are still steps to be checked
            while (note.Count != 0 && newNote.Count != 0) 
            {
                ///counter variable for outputs
                int a = 0;

                while (a < step.Outputs.Length)
                {
                    ///identify the connector at the respective output
                    var connector = step.Outputs[a];
                    var newConnector = newStep.Outputs[a];


                    if (connector != end && newConnector != newEnd)  
                    {
                        if (connector != failed && newConnector != newFailed) 
                        {
                            ///identify the following step 
                            var follower = this.Steps.FirstOrDefault(x => x.Inputs.Any(y => y.Equals(connector)));
                            var newFollower = newPlan.Steps.FirstOrDefault(x => x.Inputs.Any(y => y.Equals(newConnector)));

                            ///check if the following step has already been checked
                            if (!(check.Contains(follower) && newCheck.Contains(newFollower))) 
                            {

                                ///add the following step to the notelist
                                note.Add(follower);
                                newNote.Add(newFollower);
                            }
                        }
                        else 
                        {
                            if (connector.Classification != newConnector.Classification)
                            {
                                return false;
                            }
                        }
                    }

                    else 

                   {
                        if (connector.Classification != newConnector.Classification)
                        {
                            return false;
                        }
                    }
                    a++;
                }

                ///compare steps
                if (step.GetType() == newStep.GetType())
                {
                    ///the compared step is removed from the notelist
                    note.RemoveAll(x => x.Equals(step));
                    newNote.RemoveAll(x => x.Equals(newStep));

                    ///and add to the checklist
                    check.Add(step);
                    newCheck.Add(newStep);

                    if (note.Count != 0 && newNote.Count != 0)
                    {
                        ///select the next step in the notelist  
                        step = note[0];
                        newStep = newNote[0];
                    }
                }
                else
                {
                    return false;
                }

            }
            ///both workplans are identical 
            return true;
            

        }
    }
}

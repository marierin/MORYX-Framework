// Copyright (c) 2023, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.ComponentModel;
using System;

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
            if (!(obj is Workplan))
            {
                return false;
            }
            Workplan newPlan = (Workplan)obj;

            
        
            var start = this.Connectors.First(x => x.Name.Equals("Start"));
            var end = this.Connectors.First(x => x.Name.Equals("End"));
            var failed = this.Connectors.First(x => x.Name.Equals("Failed"));

            var newStart = newPlan.Connectors.First(x => x.Name.Equals("Start"));
            var newEnd = newPlan.Connectors.First(x => x.Name.Equals("End"));
            var newFailed = newPlan.Connectors.First(x => x.Name.Equals("Failed"));

            var step = this.Steps.First(x => x.Inputs.Any(y => y.Equals(start)));
            var newStep = newPlan.Steps.First(x => x.Inputs.Any(y => y.Equals(newStart)));

            List<IWorkplanStep> needsCheck = new List<IWorkplanStep>();
            List<IWorkplanStep> newNeedsCheck = new List<IWorkplanStep>();

            List<IWorkplanStep> checkd = new List<IWorkplanStep>();
            List<IWorkplanStep> newCheckd = new List<IWorkplanStep>();

            needsCheck.Add(step);
            newNeedsCheck.Add(newStep);

            while (needsCheck.Count != 0 && newNeedsCheck.Count != 0) 
            {

                for (int i = 0; i < step.Outputs.Length; i++)
                {
                    var connector = step.Outputs[i];
                    var newConnector = newStep.Outputs[i];

                    bool isNotEndConnector = (connector != end && newConnector != newEnd);
                    bool isNotFailedConnector = (connector != failed && newConnector != newFailed);

                    if (isNotEndConnector && isNotFailedConnector)
                    {
                        var followingStep = this.Steps.FirstOrDefault(x => x.Inputs.Any(y => y.Equals(connector)));
                        var newFollowingStep = newPlan.Steps.FirstOrDefault(x => x.Inputs.Any(y => y.Equals(newConnector)));

                        
                        bool isAlreadyChecked = (checkd.Contains(followingStep) && newCheckd.Contains(newFollowingStep));
                        if (!(isAlreadyChecked))
                        {

                            needsCheck.Add(followingStep);
                            newNeedsCheck.Add(newFollowingStep);
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


                bool isSameStep = (step.GetType() == newStep.GetType());
                if (isSameStep)
                {
                    needsCheck.Remove(step);
                    newNeedsCheck.Remove(newStep);

                    checkd.Add(step);
                    newCheckd.Add(newStep);

                    if (needsCheck.Count != 0 && newNeedsCheck.Count != 0)
                    {
                        step = needsCheck[0];
                        newStep = newNeedsCheck[0];
                    }
                }
                else
                {
                    return false;
                }

            }
            return true;
            

        }
    }
}

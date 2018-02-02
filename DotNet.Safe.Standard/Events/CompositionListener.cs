namespace DotNet.Safe.Standard.Events
{
    /// <summary>
    /// Default class for composition event listeners. Make your class extend from
    /// this one, then override the methods you need.
    /// </summary>
    public class CompositionListener : ICompositionListener
    {
        /// <summary>
        /// This method will be called when a composition finishes executing.
        /// </summary>
        /// <param name="sender">Source</param>
        /// <param name="args">Arguments</param>
        public virtual void OnCompositionFinished(object sender, CompositionStatus args) { }
        /// <summary>
        /// This method will be called when a composition starts to execute.
        /// </summary>
        /// <param name="sender">Source</param>
        /// <param name="args">Arguments</param>
        public virtual void OnCompositionStarted(object sender, CompositionStatus args) { }
        /// <summary>
        /// This method will be called when an otherwise step starts being invoked.
        /// </summary>
        /// <param name="sender">Source</param>
        /// <param name="args">Arguments</param>
        public virtual void OnOtherwiseBeginInvocation(object sender, OtherwiseStep args) { }
        /// <summary>
        /// This method will be called when an otherwise step finishes being invoked.
        /// </summary>
        /// <param name="sender">Source</param>
        /// <param name="args">Arguments</param>
        public virtual void OnOtherwiseEndInvocation(object sender, OtherwiseStep args) { }
        /// <summary>
        /// This method will be called when an otherwise step fails.
        /// </summary>
        /// <param name="sender">Source</param>
        /// <param name="args">Arguments</param>
        public virtual void OnOtherwiseFailure(object sender, CompositionError args) { }
        /// <summary>
        /// This method will be called when an otherwise step is ignored.
        /// </summary>
        /// <param name="sender">Source</param>
        /// <param name="args">Arguments</param>
        public virtual void OnOtherwiseIgnored(object sender, OtherwiseStep args) { }
        /// <summary>
        /// This method will be called when a composition step starts being invoked.
        /// </summary>
        /// <param name="sender">Source</param>
        /// <param name="args">Arguments</param>
        public virtual void OnStepBeginInvocation(object sender, CompositionStep args) { }
        /// <summary>
        /// This method will be called when a composition step finishes being invoked.
        /// </summary>
        /// <param name="sender">Source</param>
        /// <param name="args">Params</param>
        public virtual void OnStepEndInvocation(object sender, CompositionStep args) { }
        /// <summary>
        /// This method will be called when a composition step fails.
        /// </summary>
        /// <param name="sender">Source</param>
        /// <param name="args">Arguments</param>
        public virtual void OnStepFailure(object sender, CompositionError args) { }
        /// <summary>
        /// This method will be called when a composition step is ignored.
        /// </summary>
        /// <param name="sender">Source</param>
        /// <param name="args">Arguments</param>
        public virtual void OnStepIgnored(object sender, CompositionStep args) { }
    }
}

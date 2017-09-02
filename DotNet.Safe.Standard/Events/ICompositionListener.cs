namespace DotNet.Safe.Standard.Events
{
    /// <summary>
    /// Common interface for all event listeners
    /// </summary>
    public interface ICompositionListener
    {
        /// <summary>
        /// This method will be called when a composition step starts being invoked.
        /// </summary>
        /// <param name="sender">Source</param>
        /// <param name="args">Arguments</param>
        void OnStepBeginInvocation(object sender, CompositionStep args);
        /// <summary>
        /// This method will be called when a composition step finishes being invoked.
        /// </summary>
        /// <param name="sender">Source</param>
        /// <param name="args">Params</param>
        void OnStepEndInvocation(object sender, CompositionStep args);
        /// <summary>
        /// This method will be called when a composition step is ignored.
        /// </summary>
        /// <param name="sender">Source</param>
        /// <param name="args">Arguments</param>
        void OnStepIgnored(object sender, CompositionStep args);
        /// <summary>
        /// This method will be called when a composition step fails.
        /// </summary>
        /// <param name="sender">Source</param>
        /// <param name="args">Arguments</param>
        void OnStepFailure(object sender, CompositionError args);
        /// <summary>
        /// This method will be called when an otherwise step starts being invoked.
        /// </summary>
        /// <param name="sender">Source</param>
        /// <param name="args">Arguments</param>
        void OnOtherwiseBeginInvocation(object sender, OtherwiseStep args);
        /// <summary>
        /// This method will be called when an otherwise step finishes being invoked.
        /// </summary>
        /// <param name="sender">Source</param>
        /// <param name="args">Arguments</param>
        void OnOtherwiseEndInvocation(object sender, OtherwiseStep args);
        /// <summary>
        /// This method will be called when an otherwise step is ignored.
        /// </summary>
        /// <param name="sender">Source</param>
        /// <param name="args">Arguments</param>
        void OnOtherwiseIgnored(object sender, OtherwiseStep args);
        /// <summary>
        /// This method will be called when an otherwise step fails.
        /// </summary>
        /// <param name="sender">Source</param>
        /// <param name="args">Arguments</param>
        void OnOtherwiseFailure(object sender, CompositionError args);
        /// <summary>
        /// This method will be called when a composition starts to execute.
        /// </summary>
        /// <param name="sender">Source</param>
        /// <param name="args">Arguments</param>
        void OnCompositionStarted(object sender, CompositionStatus args);
        /// <summary>
        /// This method will be called when a composition finishes executing.
        /// </summary>
        /// <param name="sender">Source</param>
        /// <param name="args">Arguments</param>
        void OnCompositionFinished(object sender, CompositionStatus args);
    }
}

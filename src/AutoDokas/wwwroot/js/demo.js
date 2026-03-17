window.demoTour = {
    start: function (dotnetRef, steps) {
        var introSteps = [];
        for (var i = 0; i < steps.length; i++) {
            var el = document.querySelector(steps[i].selector);
            if (!el) continue;
            introSteps.push({
                element: el,
                title: steps[i].title,
                intro: steps[i].description,
                position: 'bottom'
            });
        }

        if (introSteps.length === 0) return;

        window.scrollTo({ top: 0, behavior: 'instant' });

        var tour = introJs.tour();
        tour.setOptions({
            steps: introSteps,
            showStepNumbers: false,
            showBullets: true,
            exitOnOverlayClick: false,
            disableInteraction: true,
            scrollToElement: false,
            nextLabel: 'Toliau \u2192',
            prevLabel: '\u2190 Atgal',
            doneLabel: 'Baigti',
            stepNumbersOfLabel: 'iš',
            tooltipClass: 'demo-tooltip'
        });
        var observer = null;

        function clearInlinePos(tooltip) {
            tooltip.style.removeProperty('inset');
            tooltip.style.removeProperty('margin-left');
            tooltip.style.removeProperty('margin-top');
            tooltip.style.setProperty('opacity', '1');
        }

        function watchTooltip(tooltip) {
            if (observer) observer.disconnect();
            observer = new MutationObserver(function () {
                if (tooltip.style.getPropertyValue('inset') ||
                    tooltip.style.getPropertyValue('margin-left')) {
                    clearInlinePos(tooltip);
                }
            });
            observer.observe(tooltip, { attributes: true, attributeFilter: ['style'] });
        }

        tour.onafterchange(function (targetElement) {
            setTimeout(function () {
                targetElement.scrollIntoView({ behavior: 'smooth', block: 'center' });
                var tooltip = document.querySelector('.introjs-tooltip.demo-tooltip');
                if (tooltip) {
                    clearInlinePos(tooltip);
                    watchTooltip(tooltip);
                }
            }, 50);
        });
        tour.oncomplete(function () {
            if (observer) observer.disconnect();
            if (dotnetRef) {
                dotnetRef.invokeMethodAsync('OnTourComplete');
            }
        });
        tour.onexit(function () {
            if (observer) observer.disconnect();
            if (dotnetRef) {
                dotnetRef.invokeMethodAsync('OnTourComplete');
            }
        });
        tour.start();
    }
};
